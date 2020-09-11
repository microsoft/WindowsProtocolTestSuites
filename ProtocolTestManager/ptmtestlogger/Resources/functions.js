// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

var filter = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAH8AAAAXCAYAAAAiGpAkAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsIAAA7CARUoSoAAAADjSURBVGhD7doxCoQwEAXQiYm5lhbiXsLDeBf7tJZeSGzcTjHLhCwry8KmnvkPJHEsPx9RYvb9GQlUqvIKCpl1XeOyLHRdVx6BZMYY6rqO6tqTmaYpDsOQH4EGIQTq+wdV3vs8Ai3O80wr3vmKIXzFEL5iCF8xhC9A27bpE+5+8ewfhC/AOI559/Fr9g3hC8Atb5om31Hao/mK3Jte0nqG8IV4t7+09QzhC8KNL209Q/iCcONLW88QvmIIXzGErxjCV6ziX4Ggy3EcaTXbtsV5nsk5lwYgm7U2HeOy1hFO76pF9AK1pjdhWLkJdwAAAABJRU5ErkJggg==";
var spread = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAkAAAAJCAIAAABv85FHAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAfSURBVBhXY1iKG4Dk/mMDlMuFogIUOUxAoRx2sHQpAKV9xlhCkKRRAAAAAElFTkSuQmCC";
var fold = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAkAAAAJCAIAAABv85FHAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAtSURBVBhXY1iKG4DkHmED6HKhoaFQFrFyQFFkgCIHARBRCCBFDhlA5bCDpUsBCAewyPpzLswAAAAASUVORK5CYII=";
var Inconclusive = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABIAAAAYCAMAAAG3jG2VAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAG5UExURQAAAOviOPPmGPHjEe7fIvTnEO/fKvLlMfHiAO7hC+vXJ+nNAOvfLPPmQOXZBvXsS+rOAPLjHPPrWPPsPevhLPDdAPHkNfbvSezQAPLqQO3tT+XYX+fKAO3RAO3tV/XmGezdJfLYAOfbJu7SAPTvLfDUAO/kL/HVAPbtOO7jMvDhK+7uM/buGfPvdezgKfDhMuXfeeDGB+vUAOXfaOvlOfHoO+vlP/LqOubgaO7fGOzdGPXwSvTvMenPBPXwUuziKunPAPTuuPHjIubfPuzaC+/YAObUAPDvfvPrNf7xNevSAPHqNe7rZvTqOe7gJf///+zsP+jMAfPzN+jMBOvaEO3dAO3UAOjOAPLmIO/nR/fwVPDwefXvQ/TyeurPAObdQu/nT+nfL+3fMe7hNti/AO/hE/TuO+zRAPHYAPXtJebbIujfS+fLAOvXBOveKOnMAO7TAPPyzvjujO/lIPbuROrfHubbX+nSBPfvR+jopPLgGvTrIuvVAObZUPXjDO3jKfTuI/XjFvPZAevaDO7mOff3avPubu7mQebLAu/ZAOrRAObVAOraGPLqOenlbuHGCOrXAPDjGuvYALOow70AAACTdFJOUwDj/4H//zD//94N/8T//xv//0LrNMETyP8/HSj4/x3/Nv///5H/MP87eSMP/0G+Iyj4/7IoiSh6sr1x9Zay9TaM/8H/zP////ET/8T/Mf//HLIXss3///jOIEVFh4z//yAwOT346ev//4f///g0xP////+EyP//siD///f////lx/+szB8kLh+y/4z/ce3/+Ln/ubwTSvMAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAEISURBVBhXYwADRSCuhDADGLi5gVQriC3qzwkkTaTNQBw4aEgDEsoFYgyTuLknMLQYGLRDxGEgqjoawpBkKuLQBTGSJkr4S4QmA1l5MjIpMjI8IEECQEoOymAQL4EyhOQbYyEsb3beCDBDQ9C5r8ocxNJL786SCAEy2Fi4S7O5/dQYGETCc/oNZWRygYImBtLSBqjeIAUkxDRlRkLZEMCvYldnbdMG5YFAkKlHhqq6jpMvlM/A4Mnc2+nj7+4qXFwBFdHWTA2T5fOPl01ntUwEi3BZ1dfExbkVKsXFGTH2cIGE7F288vO5ax3LuLm5BZoVQEJaXYGB5cHGFsbBth0dDvogIcKAgQEADMkwnizPP+8AAAAASUVORK5CYII=";
var Passed = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABYAAAAUCAMAAAHJpQ2UAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAHLUExURQAAAKzVrD+ePzaaNu327SOSIzKZMjCXMDOZM5HIkSyWLDWaNff7953OnT6ePpTKlDGXMT+fP6TRpDOaMz+fPzScNHa6djecN0GgQSSRJKfWpzGbMbjbuDKZMiWSJTOZM/D38DKYMi6WLjOYM5TJlDScNEGfQTCYMCqUKjKYMjWhNTSWNDGZMTWaNXq8ejKYMjWaNdfr1zKZMjKZMjKYMjSZNE2mTS2WLTeXNzKYMjGYMer06jSdNDOaMzOZM/T59CqVKjSWNDKZMjOZMzKZMlupWzGZMTSaNP///zOZM0aiRtHo0TGVMdrs2jKYMqzVrDCRMH6+fkKgQjKaMn+/f5fHl0CgQFupWzSWNDKZMjadNiSSJO/375bFliWTJTKYMjKZMvj7+DCXMDebN5zNnDOZM5fOlzKYMi+YL8vlyzOaMzCZMDOaM3i7eHC4cKfTp0KgQp7OnpPJkzKYMkOhQyaSJrHYsSeTJzCXMDOZMzebNyiUKDKZMjGYMZnMmTKaMimVKTqcOpfLlzKZMjKaMiqWKjOZMzOaM6nTqUShRDKYMiybLHO5czSbNEWiRTaXNkOgQ6vVqzKaMjKYMuLw4rTZtDKYMiqUKkGiQUQZxeYAAACZdFJOUwAlfKr//7D5+P////8V/4S0ff93/yf//4T/Jin/tf+p/7EW+f8sff8Y+hMi/yv/Ga3/ebrctf8nJbav/yKB5P//J7f/+CqK//+z//8p//z/Ff9/f/8l4CcsvCr//xb/I4P/+in/+SV///+x/9/////gJSbj4P////8oLv/k/yiv/////4n/4P////sX////KuH/rK///93/hPqyR5EAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAFBSURBVBhXYwADBxcGBlMIU9kaSMgCcUu7Az+Q6kqUUwaJg0G7ewMDQ9R0sXJzBmnJGGcLBgZBTyOwTEKkcWQQmCWt6excah/IXghkRymnGE7Rc3ZxBElIiFc3uDung5g4Qb+ssbGAFJiZbNferhM3oQjEZmt3du5RaGUGsS3do6NZ9Nwng9kNHh0spe31ILaVO6dHrnOvPIjNGJbNqtM7zQbEZigy8PScFA9mYgB91SoQiOiG8sHAx63GTxME/NpmpEHFGDRsi9uBrnCu6GvybeIqYOKFCOs6pYJE1UUmeshwN7tkhUOEoyyVuULy87z8PaY21ra71IMDAKy6tjPTg8ejjE/duZ1DURQirKSWo5NhJuSt4lXh3O5sIqwFEWZgCOUvSVLm4OBQ1pwWrA0VA4PYOlfp5OT+ygAoHytgYAAAQF5KkPS9yX0AAAAASUVORK5CYII=";
var Failed = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABYAAAAUCAMAAAHJpQ2UAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAADzUExURQAAAOtFNetFNu1GNOlENeYVA+9PP+lCNOo9MepBNOYUAektG+YVAfBGOOcpF+YWAew9L+gqGOg+MfKLgec0I+YUAOYdCuURAPJKPudGOOc/L/739uURAPWpoucTAOg8MeUWAve0rvBBMus+MPi+uew+MfKHff////nIw/ORiOtBMucjEeckEuYdCulCNO5CN+UXBOUUAOYVAeUYBeUWA+QQAOUUAOksGuYeC/3u7eQQAPGAdec2Jf749+USAOpIPuUUAeUTAOcmFOs1JOQPAPi/uukoFutDNeYRAPnJxOopF+QUAOcoFeUbCOckEeszIegTACouQoAAAABOdFJOUwD//x2ZjyCkPrqT/7ok/7M2/zn/w///ySk2IP////8u+v8zNf8p/////zP/yu6zLvjJm/H09v/////z/8T//zGV+P//////nv////rL8cFJdy4AAAAJcEhZcwAADsMAAA7DAcdvqGQAAAEWSURBVBhXdY/XcsIwEEWX3gwGg3EAU4wNSw9V9GaKDaH9/9dEks3kIcmZ0erslbQzAk6dAPQcVQq0iHTlprZFtwxqL5ZzxnKM1g1mdQDvA1sAkrjjJ+GIIAy5wcqDpoGlKnNFbeJ6Ruh1gO3oLNtXP9P/GQpCJOyoFbPl6TjHtFpCw0TPirlOsgWsbxTHZ2tsqtxbL9Qe6M0wB//Vls+jLXfYiWJPcvQX8fInpRx3WwcpNKl8USqT0M+7fPR+uDQSqppoXA73aN6N9Q5pLxE1DXHZJh3+UxYPCDmm5ojz1JGQwTtO3oiZRiwWEdMmuSXd2BfoPk+GqaqmcXp2Az43pmOsRX9P6S+s9wiHYO2DUgu67Z8AfAMkISUJElTKAgAAAABJRU5ErkJggg==";
var selectedCaseDiv = 0;  // The variable is used to save the object of the selected case in the left side.
var filterDisplayed = false; // The boolean is used to indicate if the "Log filter" is displayed or not.

// Used by Main.html
// Set height of iframe, the bottoms of left_sidebar and iframe are in the same line
function SetIFrameHeight(frame)
{
    if (frame)
    {
        frame.height = document.getElementById("left_sidebar").clientHeight - document.getElementById("summary").clientHeight;
    }
}

// Generate case list in left_sidebar
function CreateCaseList()
{
    var caseListOjb = document.getElementById("list");
    caseListOjb.style.marginLeft = "15px";
    var len = listObj['TestCases'].length;

    for (var i = 0; i < len; i++)
        caseListOjb.appendChild(CreateOneCase(i));

    // Set height of left side.
    document.getElementById("left_sidebar").style.height = (window.screen.height - 150) + 'px';
}

// Generate one case for case list in left_sidebar
function CreateOneCase(id)
{
    var casediv = document.createElement("div");
    var caseTextDiv = document.createElement("div");
    var casetext = document.createElement("a");
    var result = listObj['TestCases'][id]['Result'];
    var backgroundImg;
    switch (result) {
        case "Failed":
            backgroundImg = Failed;
            break;
        case "Passed":
            backgroundImg = Passed;
            break;
        case "Inconclusive":
            backgroundImg = Inconclusive;
            break;
        case "NotFound":
            backgroundImg = NotFound;
            break;
    }

    var index = document.getElementById("list").children.length;
    casediv.onclick = function () { ClickChange(this, listObj['TestCases'][id]['Name']); }

    var casename = listObj['TestCases'][id]['Name'];
    CreateText(casetext, casename);

    // Set the img as background, then the text (case name) will not drop to next line if it's too long.
    caseTextDiv.style.background = "url(" + backgroundImg + ") no-repeat left";
    caseTextDiv.appendChild(casetext);
    casetext.style.paddingLeft = '20px';
    caseTextDiv.style.padding = "6px 0px 3px 5px";

    casediv.appendChild(caseTextDiv);
    return casediv;
}

function ClickChange(caseDiv, casename)
{
    // Change src dynamically
    document.getElementById("testcase").src = 'Html/' + casename + '.html';

    document.getElementById('back_to_summary').style.display = '';
    document.getElementById('summary').style.display = '';
    document.getElementById('right_sidebar_case').style.display = '';
    document.getElementById("right_sidebar_summary").style.display = 'none';

    // Hightlight the selected case
    caseDiv.className = 'highLight';

    // Recover the old selected case
    if (selectedCaseDiv)
    {
        selectedCaseDiv.className = 'normal';
    }
    selectedCaseDiv = caseDiv;
}

// Group test cases
// Change style depends on group in left_sidebar
function ChangeSelect(value)
{
    var showlist = document.getElementById("list");
    var keyword = document.getElementById("keyword").value;
    var html = "";
    var len = listObj['TestCases'].length;
    showlist.innerHTML = "";
    if (value == 1)
    {
        for (var i = 0; i < len; i++)
        {
            if (listObj['TestCases'][i]['Name'].toLowerCase().indexOf(keyword.toLowerCase()) != -1)
                showlist.appendChild(CreateOneCase(i));
        }
    }
    if (value == 2)
    {
        var failedNum = 0;
        var inconclusiveNum = 0;
        var passedNum = 0;
        var fhtml = document.createElement("div");
        var ihtml = document.createElement("div");
        var phtml = document.createElement("div");
        var nhtml = document.createElement("div");
        fhtml.id = "f";
        ihtml.id = "i";
        phtml.id = "p";
        for (var i = 0; i < len; i++)
        {
            if (listObj['TestCases'][i]['Result'] == "Failed"
                && listObj['TestCases'][i]['Name'].toLowerCase().indexOf(keyword.toLowerCase()) != -1)
            {
                failedNum = failedNum + 1;
                fhtml.appendChild(CreateOneCase(i));
            }
            if (listObj['TestCases'][i]['Result'] == "Inconclusive"
                && listObj['TestCases'][i]['Name'].toLowerCase().indexOf(keyword.toLowerCase()) != -1)
            {
                inconclusiveNum = inconclusiveNum + 1;
                ihtml.appendChild(CreateOneCase(i));
            }
            if (listObj['TestCases'][i]['Result'] == "Passed"
                && listObj['TestCases'][i]['Name'].toLowerCase().indexOf(keyword.toLowerCase()) != -1)
            {
                passedNum = passedNum + 1;
                phtml.appendChild(CreateOneCase(i));
            }
        }
        if (failedNum != 0) {
            var fframe = document.createElement("div");
            var fimg = document.createElement("img");
            fframe.className = "frame";
            fimg.onclick = function () { ChangeStyle("f") };
            fimg.className = "small";
            fimg.src = fold;
            fimg.id = "imgf";
            fframe.appendChild(fimg);
            showlist.appendChild(fframe);
            fframe.appendChild(document.createTextNode("Failed (" + failedNum + ")"));
            showlist.appendChild(fhtml);
            showlist.appendChild(document.createElement("br"));
            fhtml.style.display = 'none';
        }
        if (inconclusiveNum != 0) {
            var iframe = document.createElement("div");
            var iimg = document.createElement("img");
            iframe.className = "frame";
            iimg.onclick = function () { ChangeStyle("i") };
            iimg.className = "small";
            iimg.src = fold;
            iimg.id = "imgi";
            iframe.appendChild(iimg);
            showlist.appendChild(iframe);
            iframe.appendChild(document.createTextNode("Inconlusive (" + inconclusiveNum + ")"));
            showlist.appendChild(ihtml);
            showlist.appendChild(document.createElement("br"));
            ihtml.style.display = 'none';
        }
        if (passedNum != 0) {
            var pframe = document.createElement("div");
            var pimg = document.createElement("img");
            pframe.className = "frame";
            pimg.className = "small";
            pimg.src = fold;
            pimg.id = "imgp";
            pimg.onclick = function () { ChangeStyle("p") };
            pframe.appendChild(pimg);
            pframe.appendChild(document.createTextNode("Passed (" + passedNum + ")"));
            showlist.appendChild(pframe);
            showlist.appendChild(phtml);
            phtml.style.display = 'none';
        }
    }
    if (value == 3 || value == 4)
    {
        GroupBySelect(value, len, keyword, showlist);
    }
}

// for group by class and category
function GroupBySelect(value, len, keyword, showlist)
{
    var name;
    if (value == 3) name = "TestCasesCategories";
    if (value == 4) name = "TestCasesClasses";
    var ctglen = listObj[name].length;
    var ctgs = new Array();
    var ctgfrms = new Array();

    for (var i = 0; i < ctglen; i++)
    {
        ctgfrms[i] = document.createElement("div");
        ctgfrms[i].className = "frame";
        var ctgimg = document.createElement("img");
        ctgimg.className = "small";
        ctgimg.src = fold;
        ctgimg.id = "img" + i;
        ctgimg.onclick = function () { ChangeStyle(this.id.substring(3)) };
        ctgfrms[i].appendChild(ctgimg);
        ctgfrms[i].appendChild(document.createTextNode(listObj[name][i]));
        ctgs[i] = document.createElement("div");
        ctgs[i].id = i;
    }
    if (value == 3) name = "Category";
    if (value == 4) name = "ClassType";
    for (var i = 0; i < len; i++)
    {
        if (listObj['TestCases'][i]['Name'].toLowerCase().indexOf(keyword.toLowerCase()) == -1)
        {
            continue;
        }
        if (value == 3)
        {
            for (var j = 0; j < listObj['TestCases'][i][name].length; j++)
            {
                var id = FindIndexofCategory(listObj['TestCases'][i][name][j]);
                ctgs[id].appendChild(CreateOneCase(i));
            }
        }
        if (value == 4)
        {
            var id = FindIndexofClass(listObj['TestCases'][i][name]);
            ctgs[id].appendChild(CreateOneCase(i));
        }
    }

    for (var i = 0; i < ctglen; i++)
    {
        showlist.appendChild(ctgfrms[i]);
        showlist.appendChild(ctgs[i]);
        showlist.appendChild(document.createElement("br"));
        ctgs[i].style.display = 'none';
    }
}

// Find index of ctg in listObj['testCasesCategories']
function FindIndexofCategory(category)
{
    for (var i = 0; i < listObj['TestCasesCategories'].length; i++)
    {
        if (category == listObj['TestCasesCategories'][i])
        {
            return i;
        }
    }
    return (-1);
}

// Filter by keyword,get "enter" by code id,value is the keyword
function KeyShow(code, value)
{
    if (code == 13)
    {
        var g = document.getElementById("groupbyid");
        var id = g.options[g.selectedIndex].value;
        ChangeSelect(id);
    }
}

// Call keyword filter by press button "Go"
function KeywordGo()
{
    var g = document.getElementById("keyword").value;
    KeyShow(13, g);
}

// Find index of cls in listObj['testCasesClassses']
function FindIndexofClass(cls)
{
    for (var i = 0; i < listObj['TestCasesClasses'].length; i++)
    {
        if (cls == listObj['TestCasesClasses'][i])
        {
            return i;
        }
    }
    return (-1);
}

// Show summary of the test run
function ShowSummary()
{
    var totalCount = listObj.TestCases.length;
    var passedCount = listObj.TestCases.filter(function (tc) { return tc.Result === "Passed" }).length;
    var failedCount = listObj.TestCases.filter(function (tc) { return tc.Result === "Failed" }).length;
    var inconclusiveCount = listObj.TestCases.filter(function (tc) { return tc.Result === "Inconclusive" }).length;
    var passRate = (totalCount === 0 ? 0 : passedCount / totalCount * 100).toFixed(2);

    var tbody = document.getElementById("tableid");
    var newrow = tbody.insertRow(-1);

    CreateText(newrow.insertCell(0), totalCount);
    CreateText(newrow.insertCell(1), passedCount);
    CreateText(newrow.insertCell(2), failedCount);
    CreateText(newrow.insertCell(3), inconclusiveCount);
    CreateText(newrow.insertCell(4), passRate + "%");
}

// Used by [casename].html
function MulselShow()
{
    var typesdiv = document.getElementById("mulsel");
    typesdiv.innerHTML = "";
    if (filterDisplayed) {
        typesdiv.style.display = 'none';
        filterDisplayed = false;
        return;
    }
    else {
        typesdiv.style.display = '';
        filterDisplayed = true;
    }

    var title = GetText(document.getElementById("right_sidebar_case_title"));
    var html = '';
    var sltAlllab = document.createElement("label");
    var sltAllinput = document.createElement("input");
    sltAllinput.type = "checkbox";
    sltAllinput.value = "(Select All)";
    sltAllinput.onclick = function () { SelectALL(this.checked); }
    CreateText(sltAlllab, "(Select All)");
    typesdiv.appendChild(sltAllinput);
    typesdiv.appendChild(sltAlllab);
    sltAllinput.checked = true;
    typesdiv.appendChild(document.createElement("br"));
    var confirmdiv = document.createElement("div");
    var confirminput = document.createElement("input");
    len = detailObj['StandardOutTypes'].length;
    for (var j = 0; j < len; j++)
    {
        var typelab = document.createElement("label");
        var typeinput = document.createElement("input");
        typeinput.name = "box";
        typeinput.type = "checkbox"
        typeinput.value = detailObj['StandardOutTypes'][j];
        CreateText(typelab, detailObj['StandardOutTypes'][j]);
        typesdiv.appendChild(typeinput);
        typesdiv.appendChild(typelab);
        typesdiv.appendChild(document.createElement("br"));
        typeinput.checked = true;
    }
    confirmdiv.align = "right";
    confirminput.type = "button"
    confirminput.style.width = "auto";
    confirminput.style.height = "auto";
    confirminput.value = "OK";
    confirminput.style.fontSize = '0.75rem';
    confirminput.onclick = function () { FilterLog(this); }
    confirmdiv.appendChild(confirminput);
    typesdiv.appendChild(confirmdiv);

}

// back to summary
function ClickSummary(obj)
{
    obj.style.display = 'none';
    document.getElementById("right_sidebar_case").style.display = 'none';
    document.getElementById("right_sidebar_summary").style.display = '';
}


// Select ALL in log filter
function SelectALL(checked)
{
    var mulsel = document.getElementById('mulsel');
    for (var i = 1; i < mulsel.children.length; i++)
    {
        if (mulsel.children[i].type == "checkbox")
            mulsel.children[i].checked = checked;
    }
}

// Get text content compatible with browser
function GetText(elem)
{
    if (elem.textContent && typeof (elem.textContent) != "undefined")
        return elem.textContent;
    else
        return elem.innerText;
}

// show detail in right_sidebar_case
function ShowDetail(caseDetail, logTypes)
{
    var html = '';
    var len = 0;
    var title = GetText(document.getElementById("right_sidebar_case_title"));
    switch (caseDetail)
    {
        case "time":
            var caseTime = document.getElementById("casetime");
            caseTime.innerHTML = "";
            var pStartTime = document.createElement("p");
            var pEndTime = document.createElement("p");
            var pResult = document.createElement("p");
            pStartTime.innerHTML = 'Start Time: ' + detailObj['StartTime'];
            pEndTime.innerHTML = 'End Time: ' + detailObj['EndTime'];
            pResult.innerHTML = 'Result: ' + detailObj['Result'];
            caseTime.appendChild(pStartTime);
            caseTime.appendChild(pEndTime);
            caseTime.appendChild(pResult);

            var pCapture = document.createElement("u");
            pCapture.style.color = "#1382CE"; //highlight capture file link
            pCapture.onclick = function ()
            {
                if (detailObj['CapturePath'] == null)
                {
                    alert("Sorry for no capture.");
                }
                else
                {
                    if (navigator.userAgent.indexOf(".NET") > 0)
                    {
                        var activeObj = new ActiveXObject('WScript.shell');
                        try
                        {
                            activeObj.run(detailObj['CapturePath'], 1);
                        }
                        catch (err)
                        {
                            alert("Please install Microsoft Message Analyzer firstly to open this capture.\r\n"
                            + "The capture file can be found at \"" + detailObj['CapturePath'] + "\".");
                        }
                    }
                    else
                    {
                        alert("Incompatibility Problem! \r\nPlease use IE to open this index.html");
                    }
                }
            }

            if (detailObj['CapturePath'] != null)
            {
                caseTime.appendChild(document.createTextNode("Capture: "));
                caseTime.appendChild(pCapture);
                CreateText(pCapture, title + '.etl');
            }

            caseTime.style.fontFamily = "Tahoma";

            break;
        case "StandardOut":
            var html = '';
            len = detailObj[caseDetail].length;
            for (var j = 0; j < len; j++)
            {
                if (logTypes == '0' || (logTypes != '0' && logTypes.indexOf(detailObj[caseDetail][j]['Type']) != -1))
                {
                    if (detailObj[caseDetail][j]['Type'].toLowerCase().indexOf('succee') != -1
                        || detailObj[caseDetail][j]['Type'].toLowerCase().indexOf('passed') != -1)
                        html += "<p style=\"background-color:rgb(0,255,0)\">" + escapeHtml(detailObj[caseDetail][j]['Content']);
                    else if (detailObj[caseDetail][j]['Type'] == 'TestStep')
                        html += "<p style=\"background-color:rgb(0,255,255)\">" + escapeHtml(detailObj[caseDetail][j]['Content']);
                    else if (detailObj[caseDetail][j]['Type'].toLowerCase().indexOf('failed') != -1)
                        html += "<p style=\"background-color:red;color:white\">" + escapeHtml(detailObj[caseDetail][j]['Content']);
                    else html += "<p>" + escapeHtml(detailObj[caseDetail][j]['Content']);
                }
            }
            html += html == "" ? "" : "</p>";
            len = detailObj['StandardOutTypes'].length;
            document.getElementById("tout").style.display = len == 0 ? 'none' : '';
            document.getElementById("out").style.display = len == 0 ? 'none' : '';
            document.getElementById("standardout").innerHTML = html;
            break;
        case "ErrorStackTrace":
            var html = '';
            len = detailObj[caseDetail].length;
            for (var j = 0; j < len; j++)
                html += "<p>" + escapeHtml(detailObj[caseDetail][j]);
            html += html == "" ? "" : "</p>";
            document.getElementById("ttrace").style.display = html == "" ? 'none' : '';
            document.getElementById("AfterErrorStackTrace").style.display = html == "" ? 'none' : '';
            document.getElementById("trace").innerHTML = html;
            break;
        case "ErrorMessage":
            var html = '';
            len = detailObj[caseDetail].length;
            for (var j = 0; j < len; j++)
                html += "<p>" + escapeHtml(detailObj[caseDetail][j]);
            html += html == "" ? "" : "</p>";
            document.getElementById("tmsg").style.display = html == "" ? 'none' : '';
            document.getElementById("AfterErrorMessage").style.display = html == "" ? 'none' : '';
            document.getElementById("msg").innerHTML = html;
            break;
        default:
            break;
    }
}

// Change fold/spread image
function ChangeStyle(id)
{
    var showdiv = document.getElementById(id);
    var showimg = document.getElementById("img" + id);
    if (showdiv.style.display == '')
    {
        showdiv.style.display = 'none';
        showimg.src = fold;
    }
    else
    {
        showdiv.style.display = '';
        showimg.src = spread;
    }
}

// Set one frame in right_sidebar_case
function SetOneFrame(value)
{
    var frame = document.createElement("div");
    var img = document.createElement("img");
    switch (value)
    {
        case "ErrorStackTrace":
            frame.id = "ttrace";
            img.id = "imgtrace";
            img.onclick = function () { ChangeStyle('trace') };
            break;
        case "ErrorMessage":
            frame.id = "tmsg";
            img.id = "imgmsg";
            img.onclick = function () { ChangeStyle('msg') };
            break;
        case "StandardOut":
            frame.id = "tout";
            img.id = "imgout";
            img.onclick = function () { ChangeStyle('out') };
            break;
    }
    frame.className = "frame";
    img.className = "small";
    img.src = spread;
    frame.appendChild(img);
    frame.appendChild(document.createTextNode(value));
    return frame;
}

// Set frames in right_sidebar_case
function SetFrames()
{
    var log = document.getElementById("log");
    log.appendChild(SetOneFrame("ErrorStackTrace"));
    log.appendChild(SetContentDiv("ErrorStackTrace"));
    var newLineAfterErrorStackTrace = document.createElement("br");
    newLineAfterErrorStackTrace.id = "AfterErrorStackTrace";
    log.appendChild(newLineAfterErrorStackTrace);

    log.appendChild(SetOneFrame("ErrorMessage"));
    log.appendChild(SetContentDiv("ErrorMessage"));
    var newLineAfterErrorMessage = document.createElement("br");
    newLineAfterErrorMessage.id = "AfterErrorMessage";
    log.appendChild(newLineAfterErrorMessage);

    log.appendChild(SetOneFrame("StandardOut"));
    log.appendChild(SetContentDiv("StandardOut"));
}

// Set frame content div in right_sidebar_case
function SetContentDiv(value)
{
    var contentObj = document.createElement("div");
    switch (value)
    {
        case "ErrorStackTrace":
            contentObj.id = "trace";
            contentObj.className = "sub";
            break;
        case "ErrorMessage":
            contentObj.id = "msg";
            contentObj.className = "sub";
            break;
        case "StandardOut":
            contentObj.id = "out";
            var filterObj = document.createElement("div");
            filterObj.className = "filterframe";
            filterObj.appendChild(document.createTextNode('Log Filter '));
            var img = document.createElement("img");
            img.src = filter;
            img.height = 20;
            img.style.verticalAlign = "text-bottom";
            img.onclick = function () { MulselShow() };
            filterObj.appendChild(img);
            var filterConentObj = document.createElement("div");
            filterConentObj.id = "mulsel";
            filterConentObj.className = "filter";
            filterConentObj.style.display = 'none';
            var outcontentObj = document.createElement("div");
            outcontentObj.id = "standardout";
            outcontentObj.className = "sub";
            contentObj.appendChild(document.createElement("p"));
            contentObj.appendChild(filterObj);
            contentObj.appendChild(filterConentObj);
            contentObj.appendChild(outcontentObj);
            break;
    }
    return contentObj;
}

// Set text content compatible with browser
function CreateText(elem, textContent)
{
    elem.appendChild(document.createTextNode(textContent));
}

// Get user's choice about log filer
function FilterLog(obj)
{
    document.getElementById("mulsel").style.display = 'none';

    if (obj.value == 'OK')
    {
        var mulsel = document.getElementById('mulsel');
        var logTypes = '';
        for (var i = 0; i < mulsel.children.length; i++)
        {
            if (mulsel.children[i].type == "checkbox" &&  mulsel.children[i].checked == true)
                logTypes += mulsel.children[i].value + ',';
        }
    }
    ShowDetail('StandardOut', logTypes);
}

// Escape html special characters
function escapeHtml(text) {
    return text
         .replace(/&/g, "&amp;")
         .replace(/</g, "&lt;")
         .replace(/>/g, "&gt;")
         .replace(/"/g, "&quot;")
         .replace(/'/g, "&#039;");
 }
