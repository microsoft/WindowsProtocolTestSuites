// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// A class which contains various consistency checks on objects.
    /// </summary>
    static public class Consistency
    {       
        /// <summary>
        /// Checks the given sequence of objects for consistency, calling a number of different individual checkers.
        /// It is assumed that the rest of the DC (apart of the given objects) is in a consistent state.
        /// This method ensures that for the given set of objects, checks are called in the right order if they have
        /// dependencies.
        /// </summary>
        /// <param name="objs">A sequence of ModelObject.</param>
        public static void Check(Sequence<ModelObject> objs)
        {

            // First check for content rules as other checks my require contents in its place.
            // Because of that order, content check must be robust.
            foreach (ModelObject obj in objs)
            {
                CheckContent(obj);
            }

            // Next check for inheritance
            foreach (ModelObject obj in objs)
            {
                if (obj[StandardNames.governsId] != null)
                {
                    CheckInheritance(obj);
                }
            }
            // Now check object class well-formedness
            foreach (ModelObject obj in objs)
            {
                CheckObjectClass(obj);
            }

            // Finally check for structural rules
            foreach (ModelObject obj in objs)
            {
                if (obj[StandardNames.governsId] != null)
                {
                    CheckStructure(obj);
                }
            }
        }


        #region Content

        /// <summary>
        /// Content rules determine the mandatory and optional attributes of the class instances that are stored 
        /// in the directory.
        /// </summary>
        /// <param name="obj">The object of model.</param>
        public static void CheckContent(ModelObject obj)
        {
            SetContainer<string> mustContain = new SetContainer<string>();
            SetContainer<string> mayContain = new SetContainer<string>();
            string[] ExcludeAttributes = { "instancetype", "ntsecuritydescriptor", "objectcategory", "objectsid" };
            // Compute sets of may/must attribute
            string className = (string)obj[StandardNames.objectClass].UnderlyingValues.Last();
            Sequence<string> superClassList = GetSuperClassList(obj.dc, className);

            foreach (string classId in superClassList)
            {
                ModelObject classObj;
                if (!obj.dc.TryGetClass(classId, out classObj))
                {
                    // will be reported in a different checker
                    continue; 
                }
                mustContain.AddRange(GetAttributeSet(classObj, StandardNames.mustContain));
                mustContain.AddRange(GetAttributeSet(classObj, StandardNames.systemMustContain));
                mayContain.AddRange(GetAttributeSet(classObj, StandardNames.mayContain));
                mayContain.AddRange(GetAttributeSet(classObj, StandardNames.systemMayContain));
            }

            //Removing default excluded attributes from this list.
            foreach (string attr in ExcludeAttributes)
            {
                if (mustContain.Contains(attr))
                {
                    mustContain.Remove(attr);
                }
            }
            // Check for all attributes
            string dn = (string)obj[StandardNames.shortName];
            //For each attribute
            foreach (string attr in obj.attributes.Keys)
            {
                if (!ExcludeAttributes.Contains(attr))
                {
                    //This attribute is in must contain list.
                    if (mustContain.Contains(attr))         
                    {
                        mustContain.Remove(attr);
                    }
                    //This attribute is not in must and may contian list.
                    else if (!mayContain.Contains(attr))    
                    {
                        Checks.Fail("'{0}' must not contain attribute '{1}'", dn, attr);
                    }
                }
            }

            //If must contain list is not empty, this object does not contain some must contain attributes.
            if (mustContain.Count > 0)
            {
                Checks.Fail(
                    "'{0}' does not contain required attributes '{1}'", 
                    dn,
                    String.Join(",", mustContain.ToArray()));
            }           

        }

        static Set<string> GetAttributeSet(ModelObject obj, string attr)
        {
            Value value = obj[attr];
            if (value != null)
            {
                return new Set<string>(from uv in value.UnderlyingValues select ((string)uv).ToLower().Trim());
            }
            else
            {
                return new Set<string>();
            }
        }
        
        #endregion

        #region Structure
        
        /// <summary>
        /// If it is a structural object class, then the objectClass attribute of objects of the class does not contain
        /// the name of the auxiliary class aux.
        /// </summary>
        /// <param name="obj">The object of model.</param>
        public static void CheckStructure(ModelObject obj)
        {
            //Collect possSuperiors.
            Sequence<object> possSuperiors = new Sequence<object>();
            //Get current class name.
            string className = obj[StandardNames.ldapDisplayName].UnderlyingValues.ElementAt(0).ToString();
            //Get super class list.
            Sequence<string> superClassList = GetSuperClassList(obj.dc, className);

            //For each super class
            foreach (string superClass in superClassList)
            {
                //Get class object of this class name.
                ModelObject classObject = obj.dc.GetClass(superClass);

                //Get possSuperiors.
                if (classObject[StandardNames.possSuperiors] != null)
                {
                    possSuperiors = possSuperiors.AddRange(classObject[StandardNames.possSuperiors].UnderlyingValues);
                }

                //Get systemPossSuperiors.
                if (classObject[StandardNames.systemPossSuperiors] != null)
                {
                    possSuperiors = possSuperiors.AddRange(classObject[StandardNames.systemPossSuperiors].UnderlyingValues);
                }                    
            }

            //For each poss superior class name...
            foreach (string possSuperior in possSuperiors)
            {
                #region two

                //Get the poss superior object and possibleInferiors attribute.
                ModelObject superiorObj = obj.dc.GetClass(possSuperior);
                ModelObject attrObject = superiorObj.dc.GetAttribute(StandardNames.possibleInferiors);

                if (superiorObj != null)
                {
                    AttributeContext parsingContext = obj.dc.GetAttributeContext(StandardNames.possibleInferiors);

                    //If this class alrady contains the possibleInferiors...
                    if (superiorObj[StandardNames.possibleInferiors] != null)
                    {

                        string valueString = String.Empty;
                        foreach (object val in superiorObj[StandardNames.possibleInferiors].UnderlyingValues)
                        {
                            valueString = valueString + val.ToString() + ";"; 
                        }

                        //Append the current class name with already existing possibleInferiors attribute.
                        valueString = valueString + className;
                        superiorObj[StandardNames.possibleInferiors] = parsingContext.Parse(valueString);
                    }
                    else
                    {
                        //Create a new possibleInferiors attribute with the cureent class name.
                        superiorObj[StandardNames.possibleInferiors] = parsingContext.Parse(className);
                    }
                }
                else
                {
                    Checks.Fail(
                        "{0} class's possSuperior {1} is not in Schema NC.", 
                        className,
                        superiorObj.attributes[StandardNames.cn].UnderlyingValues.ElementAt(0).ToString());
                }

                #endregion
            }
        }
 
        #endregion

        #region Inheritance

        /// <summary>
        /// Inheritance is the ability to build new classes from existing classes.
        /// </summary>
        /// <param name="obj">The object of model.</param>
        public static void CheckInheritance(ModelObject obj)
        {
            string dn = (string)obj[StandardNames.shortName];
            //Get super class id.
            string superClassId = (string)obj[StandardNames.subClassOf];

            if (superClassId == null)
            {
                Checks.Fail("'{0}' must have a super class", dn);
            }
            else
            {
                ModelObject superClass;
                if (!obj.dc.TryGetClass(superClassId, out superClass))
                {
                    Checks.Fail("'{0}' has undefined super class '{1}'", dn, superClassId);
                }
                else
                {
                    string thisClassId = (string)obj[StandardNames.governsId];

                    if (thisClassId != StandardNames.topGovernsId && thisClassId == superClassId)
                    {
                        Checks.Fail("{0} cannot inherit from itself", dn);
                    }

                    //Get super class and current class category.
                    ObjectClassCategory thisCategory = (ObjectClassCategory)(int)obj[StandardNames.objectClassCategory];
                    ObjectClassCategory superCategory = (ObjectClassCategory)(int)superClass[StandardNames.objectClassCategory];
                    bool categoryOk;

                    switch (thisCategory)
                    {
                        //Abstract Class must be inherited from Abstract Class.
                        case ObjectClassCategory.AbstractClass:
                            categoryOk = superCategory == ObjectClassCategory.AbstractClass;
                            break;
                        //Auxiliary Class must not be inherited from Structural Class
                        case ObjectClassCategory.AuxiliaryClass:
                            categoryOk = superCategory != ObjectClassCategory.StructuralClass;
                            break;
                        //Structural Class must not be inherited from Auxiliary Class
                        case ObjectClassCategory.StructuralClass:
                            categoryOk = superCategory != ObjectClassCategory.AuxiliaryClass;
                            break;
                        //Default.
                        default:
                            categoryOk = true;
                            break;
                    }

                    if (!categoryOk)
                    {
                        Checks.Fail("{0} must have object class category which is compatible with its super class", dn);
                    }
                }
            }
        }

        #endregion

        #region Object Class

        /// <summary>
        ///  Check objectClass.
        /// </summary>
        /// <param name="obj">ModelObject.</param>
        public static void CheckObjectClass(ModelObject obj)
        {           
            
            string dn = (string)obj[StandardNames.shortName];
            Value objectClass = obj[StandardNames.objectClass];

            if (objectClass == null)
            {
                Checks.Fail("'{0}' must have object class", dn);

                return;
            }

            // Check for resolution and last/first element compliance
            int count = 0;
            Sequence<string> classes = obj.GetAllClassIds();
            bool resolveOk = true;

            foreach (string classId in classes)
            {
                ModelObject classObject;
                //The object must be present in Schema NC.
                if (!obj.dc.TryGetClass(classId, out classObject))
                {
                    Checks.Fail("'{0}' has undefined object class '{1}'", dn, classId);
                    resolveOk = false;
                    continue;
                }
                //Checking for the first class is Top
                if (count == 0 && classId != StandardNames.top)
                {
                    Checks.Fail("'{0}' first object class must be top", dn);
                    resolveOk = false;
                }
                //Checking for the last class is Structural class.
                if (count == objectClass.UnderlyingValues.Count-1)
                {
                    ObjectClassCategory category = (ObjectClassCategory)(int)classObject[StandardNames.objectClassCategory];

                    if (category != ObjectClassCategory.StructuralClass)
                    {
                        Checks.Fail("'{0}' last object class must be structural");
                        resolveOk = false;
                    }
                }
                count++;
            }
            if (!resolveOk)
            {
                return;
            }

            // Check for superclass chaining
            // Checks that if the value of object class is [top,ac_1,...,ac_n,sc_1,...,sc_n],
            // then sc_n is the most specific structural class, sc_1...scn_(n-1) is the super class chain of sc_n (excluding top),
            // and ac_n is the next auxilary class before that, where ac_1...acn(n-1) is that classes chain excluding classes
            // which have been seen already before (where there can be a number of ac anchors here).
            SetContainer<string> includedClasses = new SetContainer<string>();
            int i = classes.Count - 1;

            while (i > 0)
            {
                string classId = classes[i--];
                if (includedClasses.Contains(classId))
                {
                    Checks.Fail("'{0}' object class contains repeated entries", dn);
                    break;
                }
                includedClasses.Add(classId);
                ObjectClassCategory category = (ObjectClassCategory)(int)obj.dc.GetClass(classId)[StandardNames.objectClassCategory];
                if (i == classes.Count - 2)
                {
                    // Most specific class must be structural
                    if (category != ObjectClassCategory.StructuralClass)
                    {
                        Checks.Fail("'{0}' object class most specific class must be structural", dn);
                    }
                }

                //If the server is Pre Windows 2003.            
                foreach (string clId in GetSuperClassChain(obj.dc, classId).Revert())
                {
                    if (includedClasses.Contains(clId))
                    {
                        // Already included in a previous chain
                        continue;
                    }
                    includedClasses.Add(clId);
                    if (i <= 0)
                    {
                        Checks.Fail(
                            "'{0}' object class does not contain chain of super classes of '{1}' (classes missing)", 
                            dn, 
                            classId);
                        break;
                    }                    
                    if (classes[i--] != clId)
                    {
                        Checks.Fail(
                            "'{0}' object class does not contain chain of super classes '{1}' (found unexpected '{2}')", 
                            dn, 
                            classId, 
                            classes[i+1]);
                        break;
                    }
                }
            }
        }

        static Sequence<string> GetSuperClassChain(ModelDomainController dc, string classId)
        {
            if (classId == StandardNames.topGovernsId || classId == StandardNames.top)
            {
                return new Sequence<string>();
            }
            else
            {
                ModelObject classObject = dc.GetClass(classId);
                return GetSuperClassChain(dc, (string)classObject[StandardNames.subClassOf]).Add(classId);
            }
        }

        /// <summary>
        /// Get superclasslist according to DC and classId.
        /// </summary>
        /// <param name="dc">A controller of Model Domain.</param>
        /// <param name="classId">An unique ID of a class.</param>
        /// <returns>Returns a list of super classes.</returns>
        public static Sequence<string> GetSuperClassList(ModelDomainController dc, string classId)
        {
            if (classId == StandardNames.topGovernsId || classId == StandardNames.top)
            {
                return new Sequence<string>().Add(classId);
            }
            else
            {
                ModelObject classObject = dc.GetClass(classId);
                return GetSuperClassList(dc, (string)classObject[StandardNames.subClassOf]).Add(classId);
            }
        }

        #endregion
    }

}
