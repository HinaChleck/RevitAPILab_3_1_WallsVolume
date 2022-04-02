using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RevitAPILab_3_1_WallsVolume
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            #region Обработка отмены выбора
            IList<Reference> selectedRefs = null;
            try
            {
                selectedRefs = uidoc.Selection.PickObjects(ObjectType.Edge, "Выберите элемент по грани");//Edge - выбор по граням
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {

            }
            if (selectedRefs == null)
                return Result.Cancelled;
            #endregion

            List<Wall> selectedElements = new List<Wall>(); //опция
            double volumeParameter = 0;
            foreach (Reference selectedRef in selectedRefs)
            {
                Element selectedElement = doc.GetElement(selectedRef);
                if (selectedElement is Wall)
                {
                    selectedElements.Add((Wall)selectedElement);//опция
                    volumeParameter += selectedElement.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                }
            }   

            double volumeValue = UnitUtils.ConvertFromInternalUnits(volumeParameter, DisplayUnitType.DUT_CUBIC_METERS);
            TaskDialog.Show("Объем выбранных стен", $"Объем выбранных стен {volumeValue} м3");
            return Result.Succeeded;
                
                
            }
        }
    }

