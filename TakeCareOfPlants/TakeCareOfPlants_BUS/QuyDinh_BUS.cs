﻿using System;
using System.Collections.Generic;
using TakeCareOfPlants_DAL;
using TakeCareOfPlants_DTO;

namespace TakeCareOfPlants_BUS
{
    public static class QuyDinh_BUS
    {
        private static QuyDinh_SQL quyDinhSQL = new QuyDinh_SQL();
        private static ViTri_SQL viTriSQL = new ViTri_SQL();

        public static List<QuyDinh_DTO> QuyDinh_DTOs { get; set; }
        public static List<ViTri_DTO> ViTri_DTOs { get; set; }

        public static List<QuyDinh_DTO> GetValueQuyDinh() => quyDinhSQL.GetDataQuyDinh();

        public static List<ViTri_DTO> GetValueViTri() => viTriSQL.GetDataViTri();

        public static List<ViTri_DTO> GetAvailableViTri() => viTriSQL.GetAvailableLocation();

        public static void UpdateValueQuyDinh(List<ViTri_DTO> viTri_DTOs)
        {
            quyDinhSQL.UpdateDataQuyDinh(viTri_DTOs, QuyDinh_DTOs[1]);
        }
    }
}
