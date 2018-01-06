//acdbmgd
using Autodesk.AutoCAD.DatabaseServices;// (Database, DBPoint, Line, Spline)
using Autodesk.AutoCAD.Geometry;//(Point3d, Line3d, Curve3d)
using Autodesk.AutoCAD.Runtime;// (CommandMethodAttribute, RXObject, CommandFlag)
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.LayerManager;

//acmgd
using Autodesk.AutoCAD.ApplicationServices;// (Application, Document)
using Autodesk.AutoCAD.EditorInput;//(Editor, PromptXOptions, PromptXResult)
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.Internal.Windows;
using Autodesk.AutoCAD.Internal.Forms;
using Autodesk.AutoCAD.Windows.ToolPalette;


using Autodesk.AutoCAD.Interop;
using System;
using System.Reflection;

[assembly: CommandClass(typeof(EPDI_Plugin.CADPlugin))]
namespace EPDI_Plugin
{
    public class CADPlugin : IExtensionApplication
    {
        PaletteSet ps;//定义面板，插件的操作都在面板上

        public void Initialize()
        {
            AddMenuContent(); //添加菜单栏上项目
        }

        public void Terminate()
        {

        }

        /// <summary>
        /// 添加菜单项
        /// </summary>
        public void AddMenuContent()
        {
            try
            {

                AcadApplication acadApp = (AcadApplication)Application.AcadApplication;

                //添加根菜单
                AcadPopupMenu pMenu = acadApp.MenuGroups.Item(0).Menus.Add("EPDI_Plugin");
                AcadPopupMenuItem cMenu2 = pMenu.AddMenuItem(pMenu.Count + 1, "建筑插件", "architecturePlugin\n");
                pMenu.InsertInMenuBar(acadApp.MenuBar.Count + 1);
            }
            catch (System.Exception ex)
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.ToString());
            }
        }

        /// <summary>
        /// 添加插件面板，操作都在面板上
        /// </summary>
        public void AddPalette()
        {
            if (ps != null)
            {
                if (!ps.Visible)
                {
                    ps.Visible = true;
                }
                return;
            }

            MyControl contr = new MyControl();
            ps = new PaletteSet("插件操作面板");
            //面板Style设置
            ps.Visible = true;
            ps.Style = PaletteSetStyles.ShowTabForSingle;
            ps.Style = PaletteSetStyles.NameEditable;
            ps.Style = PaletteSetStyles.ShowPropertiesMenu;
            ps.Style = PaletteSetStyles.ShowAutoHideButton;
            ps.Style = PaletteSetStyles.ShowCloseButton;
            ps.Opacity = 90;
            ps.Dock = DockSides.Left;
            ps.MinimumSize = new System.Drawing.Size(200, 100);
            ps.Add("PaletteSet", contr);
        }

        /// <summary>
        /// 登录窗口显示，用户名、密码正确才能看到插件面板
        /// </summary>
        [CommandMethod("architecturePlugin")]
        [CommandMethod("ShowModalDialog")]
        public void ShowModalDialog()
        {
            using (MyForm form = new MyForm())
            {
                form.ShowInTaskbar = false;
                Application.ShowModalDialog(form);
                if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\n" + form.Name);
                    AddPalette();

                }
            }
        }

    }
}
