﻿using System;
using System.Windows.Forms;

namespace TakeCareOfPlants
{
    public partial class DialogSuccess_GUI : Form
    {
        private static DialogSuccess_GUI dialogSuccess;

        public DialogSuccess_GUI()
        {
            InitializeComponent();
        }

        public static DialogSuccess_GUI Instance
        {
            get {
                if (dialogSuccess == null) {
                    dialogSuccess = new DialogSuccess_GUI();
                }
                return dialogSuccess;
            }
        }

        private void DialogSuccess_GUI_Load(object sender, EventArgs e)
        {
            dialogSuccess = this;
            FormFadeTransition.ShowAsyc(dialogSuccess);
        }

        private void FormFadeTransition_TransitionEnd(object sender, EventArgs e)
        {
            Time_Show_Button.Start();
            Icon_Box.Visible = true;
            Icon_Box.Enabled = true;
        }

        private void Time_Show_Button_Tick(object sender, EventArgs e)
        {
            Time_Show_Button.Stop();
            OK_Button.Visible = true;
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            dialogSuccess = this;

            Function_GUI.HidePage(dialogSuccess);

            if (!IsFormOpen(typeof(PageLogin_GUI))) {
                PageLogin_GUI.Instance.Hide();
            }
            if (!IsFormOpen(typeof(PageSignUp_GUI))) {
                PageSignUp_GUI.Instance.Hide();
            }

            Function_GUI.ShowPage(PageMain_GUI.Instance);
        }

        public bool IsFormOpen(Type FormType)
        {
            foreach (Form form in Application.OpenForms) {
                if (form is PageSetting_GUI) {
                    return true;
                }
            }
            return false;
        }

        protected override CreateParams CreateParams
        {
            get {
                CreateParams cp = base.CreateParams;
                if (!Function_GUI.CheckAeroEnabled()) {
                    cp.ClassStyle |= (int)Function_GUI.ShadowBorder.CS_DROPSHADOW;
                }
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {

            switch (m.Msg) {
                case (int)Function_GUI.ShadowBorder.WM_NCPAINT:
                    if (Function_GUI.CheckAeroEnabled()) {
                        int v = 2;
                        Function_GUI.DwmSetWindowAttribute(Handle, 2, ref v, 4);
                        Function_GUI.MARGINS margins = new Function_GUI.MARGINS() {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        };
                        Function_GUI.DwmExtendFrameIntoClientArea(Handle, ref margins);
                    }
                    break;
                default:
                    break;
            }

            if (m.Msg == (int)Function_GUI.ShadowBorder.WM_NCLBUTTONDBLCLK) {
                m.Result = IntPtr.Zero;
                return;
            }

            base.WndProc(ref m);
            if (m.Msg == (int)Function_GUI.ShadowBorder.WM_NCHITTEST && (int)m.Result == (int)Function_GUI.ShadowBorder.HTCLIENT) {
                m.Result = (IntPtr)Function_GUI.ShadowBorder.HTCAPTION;
            }
        }
    }
}
