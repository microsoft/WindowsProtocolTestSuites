// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Some helper functions which are declared native for SE to work around bugs/improve efficiency.
    /// </summary>
    public static class NativeHelpers
    {
        /// <summary>
        /// Parse uint32 enum.
        /// </summary>
        /// <param name="type">The parsed type.</param>
        /// <param name="repr">The parsed string.</param>
        /// <param name="value">Result value.</param>
        /// <returns>Returns whether parse to uint32 successfully or not.</returns>
        public static bool TryParseUInt32Enum(Type type, string repr, ref uint value)
        {
            try
            {
                value = (uint)Enum.Parse(type, repr, true);

                return true;
            }
            catch (ArgumentException)
            {
                value = 0;

                return false;
            }
        }


        /// <summary>
        /// Unparse uint32 enum.
        /// </summary>
        /// <param name="type">The parsed type.</param>
        /// <param name="value">The parsed string.</param>
        /// <returns>Returns unparsed string.</returns>
        public static string UnparseUInt32Enum(Type type, uint value)
        {
            return Enum.Format(type, value, "g");
        }
    }


    /// <summary>
    ///  A class of constructedAttributeHelper.
    /// </summary>
    public class ConstructedAttributeHelper
    {
        #region SuperClasses

        /// <summary>
        /// SuperClassList
        /// </summary>
        public static List<string> SuperClassList;

        /// <summary>
        /// Finds super class chain of object.
        /// </summary>
        /// <param name="requiredObject">The required object.</param>
        /// <param name="isFirstTime">Whether is it the first time.</param>
        /// <param name="dcModel">The domain controller of model.</param>
        /// <returns>Returns a list of super classes.</returns>
        public List<string> GetSuperClassesList(ModelObject requiredObject, bool isFirstTime, ModelDomainController dcModel)
        {
            string tempCNName = (string)requiredObject.attributes[StandardNames.cn].UnderlyingValues.ElementAt(0);

            //If it is first time, we have to initialize this superClassList.
            if (isFirstTime)
            {
                SuperClassList = new List<string>();
            }
            //This is not first time, so add this obj name in superClassList.
            else
            {
                SuperClassList.Add(tempCNName);
            }
            //If this obj name is top, return new empty list.
            if (tempCNName.ToLower().Equals("top"))
            {
                return new List<string>();
            }
            else
            {
                //If this obj has some more subClassValue, call again this method with its subClassOf value.
                if (requiredObject.attributes.Keys.Contains("subclassof"))
                {
                    //Get the subClassOf value of this obj from model and server.
                    string subClassOfValue = (string)requiredObject.attributes["subclassof"].UnderlyingValues.ElementAt(0);
                    subClassOfValue = subClassOfValue.Replace("-", String.Empty).ToLower();
                    ModelObject subClassOfObject = null;

                    dcModel.TryGetClass(subClassOfValue, out subClassOfObject);
                    if (subClassOfObject == null)
                    {                  
                        subClassOfValue = (string)requiredObject.attributes["subclassof"].UnderlyingValues.ElementAt(0);
                        subClassOfValue = subClassOfValue.ToLower();
                        dcModel.TryGetClass(subClassOfValue, out subClassOfObject);
                    }

                    //Call SuperClasses method with this subClassOfObject.
                    GetSuperClassesList(subClassOfObject, false,dcModel);
                }
            }
            //Finally return this superClassList.
            return SuperClassList;
        }

        #endregion

        #region AuxiliaryClasses
       
        /// <summary>
        /// auxiliaryClassLsit
        /// </summary>
        public static List<string> auxiliaryClassLsit;

        /// <summary>
        /// seenAuxiliarClasses
        /// </summary>
        static List<string> seenAuxiliarClasses;

        /// <summary>
        /// Finds all Auxiliary classes.
        /// </summary>
        /// <param name="requiredObject">The required object.</param>
        /// <param name="isFirstTime">Whether is it the first time.</param>
        /// <param name="dcModel">The domain controller of model.</param>
        /// <returns>Returns a list of auxiliary classes.</returns>
        public List<string> GetAuxiliaryClassesList(ModelObject requiredObject, bool isFirstTime, ModelDomainController dcModel)
        {
            List<string> superClasses;
            //If it is first time, initialize all variables.
            if (isFirstTime)
            {
                auxiliaryClassLsit = new List<string>();
                seenAuxiliarClasses = new List<string>();
            }
            //Collect all systemauxiliaryclass of this object.
            if (requiredObject.attributes.Keys.Contains("systemauxiliaryclass"))
            {
                foreach (string element in requiredObject.attributes["systemauxiliaryclass"].UnderlyingValues)
                {
                    auxiliaryClassLsit.Add(element);
                }
            }
            //Collect all auxiliaryclass of this object.
            if (requiredObject.attributes.Keys.Contains("auxiliaryclass"))
            {
                foreach (string element in requiredObject.attributes["auxiliaryclass"].UnderlyingValues)
                {
                    auxiliaryClassLsit.Add(element);
                }
            }
            //For auxiliary classes,
            if (auxiliaryClassLsit.Count != 0)
            {
                List<string> tempAuxiliaryClassLsit = new List<string>();
                foreach (string element in auxiliaryClassLsit)
                {
                    tempAuxiliaryClassLsit.Add(element);
                }

                //For all auxiliary classes, find out their auxiliaryClass and systemAuxiliaryClass values.
                foreach (string auxClass in tempAuxiliaryClassLsit)
                {
                    //If already we did not find the auxiliary and systemAuxiliary class for this class continue.
                    if (!seenAuxiliarClasses.Contains(auxClass.ToLower()))
                    {
                        //Add this auxClass name in seenAuxiliarClasses.
                        seenAuxiliarClasses.Add(auxClass.ToLower());

                        //Find this object in server.
                        ModelObject auxClassOfObject;
                        string className = auxClass.Replace("-", String.Empty).ToLower();
                        if (dcModel.TryGetClass(className, out auxClassOfObject))
                        {
                            //Call AuxiliaryClasses method for this auxClass.
                            GetAuxiliaryClassesList(auxClassOfObject, false, dcModel);
                        }
                    }
                }
            }

            //Find superClasses of this object.
            superClasses = GetSuperClassesList(requiredObject, true, dcModel);
            //For each super class,
            foreach (string supClass in superClasses)
            {
                //If already we did not find the auxiliary and systemAuxiliary class for this class continue.
                if (!seenAuxiliarClasses.Contains(supClass.ToLower()))
                {
                    //Add this supClass name in seenAuxiliarClasses.
                    seenAuxiliarClasses.Add(supClass.ToLower());
                    //Find this object from server.
                    ModelObject supClassOfObject = null;
                    string className = supClass.Replace("-", String.Empty).ToLower();

                    dcModel.TryGetClass(className, out supClassOfObject);
                    //Remove the minus sign and retry
                    if (supClassOfObject == null)
                    {   //Modified to support Windows Server 2012
                        //Only class names in the format of, eg. msds-claimtype, can be found                   
                        if (className.Contains("msds"))
                            className = className.Insert(className.IndexOf("msds") + 4, "-");
                        else if (supClass.Contains("-"))
                            className = supClass.Remove(supClass.IndexOf('-'), 1);
                        dcModel.TryGetClass(className, out supClassOfObject);
                    }

                    //Call AuxiliaryClasses method for this supClass object.
                    GetAuxiliaryClassesList(supClassOfObject, false,dcModel);
                }
            }
            //Finally return auxiliary class list.
            return auxiliaryClassLsit;
        }

        #endregion

        #region Class Attributes

        public List<string> classAttributesList = new List<string>();
        public List<string> ClassAttributes(ModelObject obj, ModelDomainController dcModel)
        {
            List<string> superClasses;
            List<string> auxiliaryClasses;

            //Collect all mustcontain value of this object.
            if (obj.attributes.Keys.Contains("mustcontain"))
            {
                foreach (string element in obj.attributes["mustcontain"].UnderlyingValues)
                {
                    classAttributesList.Add(element);
                }
            }

            //Collect all systemmustcontain value of this object.
            if (obj.attributes.Keys.Contains("systemmustcontain"))
            {
                foreach (string element in obj.attributes["systemmustcontain"].UnderlyingValues)
                {
                    classAttributesList.Add(element);
                }
            }

            //Collect all maycontain value of this object.
            if (obj.attributes.Keys.Contains("maycontain"))
            {
                foreach (string element in obj.attributes["maycontain"].UnderlyingValues)
                {
                    classAttributesList.Add(element);
                }
            }

            //Collect all systemmaycontain value of this object.
            if (obj.attributes.Keys.Contains("systemmaycontain"))
            {
                foreach (string element in obj.attributes["systemmaycontain"].UnderlyingValues)
                {
                    classAttributesList.Add(element);
                }
            }

            //Collect this object's super class List.
            superClasses = GetSuperClassesList(obj, true, dcModel);

            //For each super class,
            foreach (string supClass in superClasses)
            {
                //Get the object from server.
                ModelObject supClassOfObject = null;
                string className = supClass.Replace("-", String.Empty).ToLower();

                dcModel.TryGetClass(className, out supClassOfObject);
                //Remove the minus sign and retry
                if (supClassOfObject == null)
                {   //Modified to support Windows Server 2012
                    //Only class names in the format of, eg. msds-claimtype, can be found                   
                    if (className.Contains("msds"))
                        className = className.Insert(className.IndexOf("msds") + 4, "-");
                    else if (supClass.Contains("-"))
                        className = supClass.Remove(supClass.IndexOf('-'), 1);
                    dcModel.TryGetClass(className, out supClassOfObject);
                }                

                //Call ClassAttributes method with this super class object.
                ClassAttributes(supClassOfObject, dcModel);
            }

            //Collect auxiliary classes of this object.
            auxiliaryClasses = GetAuxiliaryClassesList(obj, true, dcModel);

            //For each auxiliary class,
            foreach (string auxClass in auxiliaryClasses)
            {
                //Get the object from server.
                ModelObject auxClassOfObject;
                dcModel.TryGetClass(auxClass, out auxClassOfObject);

                //Call ClassAttributes method with this auxiliary class object.
                ClassAttributes(auxClassOfObject, dcModel);
            }
            //Finally return the classAttributes list.
            return classAttributesList;
        }


        #endregion

        #region PossSuperiors
        
        /// <summary>
        /// possSuperiorList
        /// </summary>
        public List<string> possSuperiorList = new List<string>();

        /// <summary>
        /// Finds possSuperiors of a given object.
        /// </summary>
        /// <param name="requiredObject">The required object.</param>
        /// <param name="dcModel">The domain controller of model.</param>
        /// <returns>Returns a list of poss superior.</returns>
        public List<string> GetPossSuperiorsList(ModelObject requiredObject, ModelDomainController dcModel)
        {
            //DirectoryEntry serverObj = null;
            List<string> superClasses;
            List<string> auxiliaryClasses;

            //Collect all systemPossSuperiors value of this object.
            if (requiredObject.attributes.Keys.Contains("systemposssuperiors"))
            {
                foreach (string element in requiredObject.attributes["systemposssuperiors"].UnderlyingValues)
                {
                    possSuperiorList.Add(element);
                }
            }

            //Collect all possSuperiors value of this object.
            if (requiredObject.attributes.Keys.Contains("posssuperiors"))
            {
                foreach (string element in requiredObject.attributes["posssuperiors"].UnderlyingValues)
                {
                    possSuperiorList.Add(element);
                }
            }

            //Collect this object's super class List.
            superClasses = GetSuperClassesList(requiredObject, true, dcModel);

            //For each super class,
            foreach (string supClass in superClasses)
            {
                //Get the object from server.
                ModelObject supClassOfObject = null;
                string className = supClass.Replace("-", String.Empty).ToLower();
                
                dcModel.TryGetClass(className, out supClassOfObject);      
                //Remove the minus sign and retry
                if (supClassOfObject == null)
                {   //Modified to support Windows Server 2012
                    //Only class names in the format of, eg. msds-claimtype, can be found                   
                    if (className.Contains("msds"))
                        className = className.Insert(className.IndexOf("msds") + 4, "-");                   
                    else if (supClass.Contains("-"))
                        className = supClass.Remove(supClass.IndexOf('-'), 1);
                    dcModel.TryGetClass(className, out supClassOfObject);
                }

                //Call PossSuperiors method with this super class object.
                GetPossSuperiorsList(supClassOfObject,dcModel);
            }

            //Collect auxiliary classes of this object.
            auxiliaryClasses = GetAuxiliaryClassesList(requiredObject, true, dcModel);

            //For each auxiliary class,
            foreach (string auxClass in auxiliaryClasses)
            {
                //Get the object from server.
                ModelObject auxClassOfObject;
                dcModel.TryGetClass(auxClass, out auxClassOfObject);

                //Call PossSuperiors method with this auxiliary class object.
                GetPossSuperiorsList(auxClassOfObject,dcModel);
            }
            //Finally return the possSuperior list.
            return possSuperiorList;
        }

        #endregion
    }

   
    /// <summary>
    ///  An enumeration type used to indicate which OS version the server is running.
    /// </summary>
    public enum OSVersion
    {
        OtherOS      = 0,
        WinSvr2000   = 1,
        WinSvr2003   = 2,
        WinSvr2008   = 3,
        WinSvr2008R2 = 4,
        WinSvr2012   = 5,
        WinSvr2012R2 = 6,
        Win2016 = 7,
        Winv1803 = 8,
    };
}
