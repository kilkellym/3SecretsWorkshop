#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace _3SecretsWorkshop
{
    [Transaction(TransactionMode.Manual)]
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // step 1: get a variable for the model
            Document doc = uiapp.ActiveUIDocument.Document;

            // step 2: get things from model
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_TextNotes);
            collector.WhereElementIsNotElementType();

            // step 4: create transaction
            Transaction transaction = new Transaction(doc);
            transaction.Start("Text to UPPER");

            // step 3: got through elements and update
            int counter = 0;
            foreach (TextNote currentTextNote in collector)
            {
                currentTextNote.Text = currentTextNote.Text.ToUpper();
                counter++;
            }

            transaction.Commit();
            transaction.Dispose();

            // step 5: alert user
            TaskDialog.Show("Complete", "Converted " + counter.ToString() + " text notes to UPPER.");

            return Result.Succeeded;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnCommand1";
            string buttonTitle = "Button 1";

            ButtonDataClass myButtonData1 = new ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Blue_32,
                Properties.Resources.Blue_16,
                "This is a tooltip for Button 1");

            return myButtonData1.Data;
        }
    }
}
