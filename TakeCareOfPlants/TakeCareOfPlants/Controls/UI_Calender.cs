﻿using Bunifu.Framework.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using TakeCareOfPlants_BUS;
using TakeCareOfPlants_DTO;
using Telerik.WinControls.UI;

namespace TakeCareOfPlants
{
    public partial class UI_Calender : UserControl
    {
        private static UI_Calender uiCalender;
        private Lich_BUS lichBUS = new Lich_BUS();
        private readonly VatTu_BUS vatTuBUS = new VatTu_BUS();
        private readonly CayCanh_BUS cayCanhBUS = new CayCanh_BUS();

        public static UI_Calender Instance
        {
            get {
                if (uiCalender == null) {
                    uiCalender = new UI_Calender();
                }
                return uiCalender;
            }
        }

        public List<string> ListCayCanh { get; set; } = new List<string>();
        public List<Tuple<string, int>> ListVatTu { get; set; } = new List<Tuple<string, int>>();
        public Dictionary<int, List<Tuple<string, int>>> ListComboBox { get; set; } = 
            new Dictionary<int, List<Tuple<string, int>>>();
        public List<Tuple<CayCanh_DTO, ViTri_DTO>> TuplesCayCanhViTri { get; set; }
        public List<Tuple<VatTu_DTO, DonVi_DTO>> TuplesVatTuDonVi { get; set; }

        public UI_Calender()
        {
            try {
                TuplesCayCanhViTri = cayCanhBUS.GetValueCayCanhViTri();
                TuplesVatTuDonVi = vatTuBUS.GetValueVatTuDonVi();
            } catch (Exception ex) {
                Function_GUI.ShowErrorDialog(ex.Message);
            }
            InitializeComponent();
        }

        private void UI_Calender_Load(object sender, EventArgs e)
        {
            uiCalender = this;
            Function_GUI.ChangeColorButton(Create_Button, Update_Button);
            Time_Pick.Value = DateTime.Now;

            foreach (Tuple<CayCanh_DTO, ViTri_DTO> tuple in TuplesCayCanhViTri) {
                PlantName_DropDown.Items.Add(item: new DescriptionTextCheckedListDataItem {
                    Text = tuple.Item1.Id,
                    DescriptionText = tuple.Item1.TenCay + ", " + tuple.Item2.TenViTri
                });
            }

            foreach (Tuple<VatTu_DTO, DonVi_DTO> tuple in TuplesVatTuDonVi) {
                Material_DropDown.Items.Add(item: new DescriptionTextCheckedListDataItem {
                    Text = tuple.Item1.TenVatTu,
                    DescriptionText = tuple.Item2.DonVi
                });

                ListVatTu.Add(new Tuple<string, int>(item1: tuple.Item1.Id,
                                                     item2: 0));
            }
        }

        private void Material_DropDown_ItemCheckedChanged(object sender,
                                                          RadCheckedListDataItemEventArgs e)
        {
            if (e.Item.Checked) {
                Amount_GUI.Instance.Title = e.Item.Text;
                Amount_GUI.Instance.Amount =
                    ListVatTu[e.Item.Index].Item2 != 0
                    ? ListVatTu[e.Item.Index].Item2
                    : 0;
                Amount_GUI.Instance.ShowDialog();

                switch (Amount_GUI.Instance.DialogResult) {
                    case DialogResult.OK: {
                            if (Amount_GUI.Instance.Amount != 0) {
                                ListVatTu[e.Item.RowIndex] = new Tuple<string, int>(
                                        item1: TuplesVatTuDonVi[e.Item.RowIndex].Item1.Id,
                                        item2: Amount_GUI.Instance.Amount);
                            } else {
                                e.Item.Checked = false;
                            }
                            break;
                        }
                    case DialogResult.Cancel: {
                            e.Item.Checked = false;
                            new CancelEventArgs().Cancel = true;
                            break;
                        }
                }
            }
        }

        private void PlantName_DropDown_VisualListItemFormatting(object sender,
                                                                 VisualItemFormattingEventArgs args)
        {
            if (sender == null) {
                throw new ArgumentNullException(nameof(sender));
            }

            Function_GUI.VisualListItem(PlantName_DropDown, args);
        }

        private void PlantName_DropDown_TextBlockFormatting(object sender,
                                                            TextBlockFormattingEventArgs e)
        {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            Function_GUI.TextBlock(e);
        }

        private void Material_DropDown_VisualListItemFormatting(object sender,
                                                                VisualItemFormattingEventArgs args)
        {
            if (sender == null) {
                throw new ArgumentNullException(nameof(sender));
            }

            Function_GUI.VisualListItem(Material_DropDown, args);
        }

        private void Material_DropDown_TextBlockFormatting(object sender,
                                                           TextBlockFormattingEventArgs e)
        {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            Function_GUI.TextBlock(e);
        }

        private void Create_Button_Click(object sender, EventArgs e)
        {
            //if (GlobalVariable_DTO.Lich_DTO == null) {
            //    lichBUS.InsertValueLich(new Lich_DTO(DateTime.Now));
            //    GlobalVariable_DTO.Lich_DTO.NgayLapLich = DateTime.Now;
            //} else {

            //}

            List<Tuple<string, int>> tuplesVatTu = new List<Tuple<string, int>>();

            ThoiGian_DTO thoiGianDTO = new ThoiGian_DTO(
                (Time_Pick.Value ?? DateTime.Now).TimeOfDay,
                Note_Text.Text
            );

            foreach (DescriptionTextCheckedListDataItem checkItems in PlantName_DropDown.CheckedItems) {
                ListCayCanh.Add(item: TuplesCayCanhViTri[checkItems.RowIndex].Item1.Id);
            }

            foreach (DescriptionTextCheckedListDataItem checkItems in Material_DropDown.CheckedItems) {
                tuplesVatTu.Add(new Tuple<string, int>(
                    item1: ListVatTu[checkItems.RowIndex].Item1,
                    item2: ListVatTu[checkItems.RowIndex].Item2));
            }

            ListCayCanh.Sort();
            tuplesVatTu = tuplesVatTu.OrderBy(x => x.Item1).ToList();

            //try {
            //    string idLich = lichBUS.InsertAndGetLastIdValueLich();

            //    lichBUS.InsertValueThoiGian(
            //        idLich,
            //        thoiGianDTO.ThoiGian,
            //        ListCayCanh,
            //        tuplesVatTu,
            //        thoiGianDTO.GhiChu
            //    );
            //} catch (Exception ex) {
            //    Function_GUI.ShowErrorDialog(ex.Message);
            //}

            List_Time_DataGrid.Rows.Add(
                List_Time_DataGrid.Rows.Count + 1,
                thoiGianDTO.ThoiGian.ToString(@"hh\:mm"),
                null,
                TuplesVatTuDonVi.Find(
                    f => f.Item1.Id == tuplesVatTu[0].Item1).Item2.DonVi,
                tuplesVatTu[0].Item2,
                thoiGianDTO.GhiChu);

            ComboBoxCell(List_Time_DataGrid, tuplesVatTu, List_Time_DataGrid.Rows.Count - 1, 2);

            ListComboBox.Add(List_Time_DataGrid.Rows.Count - 1, tuplesVatTu);
        }

        private void Clear_Button_Click(object sender, EventArgs e)
        {
            Function_GUI.ChangeColorButton(Create_Button, Update_Button);

            Time_Pick.Value = DateTime.Now;
            Material_DropDown.Text = "";
            Note_Text.Text = "";
            PlantName_DropDown.Text = "";
        }

        private void List_Time_DataGrid_EditingControlShowing(object sender,
                                                              DataGridViewEditingControlShowingEventArgs e)
        {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            switch (List_Time_DataGrid.CurrentCell.ColumnIndex) {
                case 2:
                    ComboBox Cb = (ComboBox)e.Control;
                    Cb.SelectedIndexChanged += new EventHandler(Material_SelectedIndexChanged);
                    break;
            }
        }

        private void Material_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currentRow = List_Time_DataGrid.CurrentRow.Index;
            var Cb = sender as ComboBox;

            List_Time_DataGrid[3, currentRow].Value = 
                TuplesVatTuDonVi.Find(
                    t => t.Item1.Id == ListComboBox[currentRow][Cb.SelectedIndex].Item1).Item2.DonVi;

            List_Time_DataGrid[4, currentRow].Value =
                ListComboBox[currentRow][Cb.SelectedIndex].Item2;
        }

        private void ComboBoxCell(BunifuCustomDataGrid customDataGrid,
                                  List<Tuple<string, int>> vatTus,
                                  int row,
                                  int column)
        {
            customDataGrid.Rows[row].Cells[column].ReadOnly = false;

            DataGridViewComboBoxCell db = (DataGridViewComboBoxCell)customDataGrid.Rows[row].Cells[column];
            db.Items.Clear();

            foreach (Tuple<string, int> VatTu in vatTus) {
                db.Items.Add(TuplesVatTuDonVi.Find(t => t.Item1.Id == VatTu.Item1).Item1.TenVatTu);
            }

            customDataGrid[column, row].Value = db.Items[0];
        }
    }
}
