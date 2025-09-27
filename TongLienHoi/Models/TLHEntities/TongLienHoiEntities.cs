using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using HTTLVN.QLTLH.Controllers;

namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CodeFirstTongLienHoiEntities : DbContext
    {
        public CodeFirstTongLienHoiEntities()
            : base("name=TLHEntities")
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<BangCapNguoiPhoiNgau> BangCapNguoiPhoiNgaus { get; set; }
        public virtual DbSet<CapChiHoi> CapChiHois { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryDetail> CategoryDetails { get; set; }
        public virtual DbSet<ChiTietChucVuGiaoPham> ChiTietChucVuGiaoPhams { get; set; }
        public virtual DbSet<ChucVu> ChucVus { get; set; }
        public virtual DbSet<Church> Churches { get; set; }

       

        public virtual DbSet<Church_Assignment_History> Church_Assignment_History { get; set; }
        public virtual DbSet<Church_SoLuongTinHuu> Church_SoLuongTinHuu { get; set; }
        public virtual DbSet<Church_TAISAN> Church_TAISAN { get; set; }
        public virtual DbSet<Church_TinHuu> Church_TinHuu { get; set; }
        public virtual DbSet<Church_TinHuu_DanToc> Church_TinHuu_DanToc { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Clergy> Clergies { get; set; }
        public virtual DbSet<Clergy_AssignmentHistory> Clergy_AssignmentHistory { get; set; }
        public virtual DbSet<Clergy_Conduct> Clergy_Conduct { get; set; }
        public virtual DbSet<Clergy_Education> Clergy_Education { get; set; }
        public virtual DbSet<Clergy_TitleHistory> Clergy_TitleHistory { get; set; }
        public virtual DbSet<ClergyTitle> ClergyTitles { get; set; }
        public virtual DbSet<Conduct_Type> Conduct_Type { get; set; }
        public virtual DbSet<DanToc> DanTocs { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<HoGiaDinh> HoGiaDinhs { get; set; }
        public virtual DbSet<HoGiaDinhTinHuu> HoGiaDinh_TinHuu { get; set; }
        public virtual DbSet<Office> Offices { get; set; }
        public virtual DbSet<PhanLoaiVanThu> PhanLoaiVanThus { get; set; }
        public virtual DbSet<QuanHeGiaDinh> QuanHeGiaDinhs { get; set; }
        public virtual DbSet<QuanHeVoiGiaoPham> QuanHeVoiGiaoPhams { get; set; }
        public virtual DbSet<RoleDetail> RoleDetails { get; set; }
        public virtual DbSet<RoleMapping> RoleMappings { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TinHuu> TinHuus { get; set; }
        public virtual DbSet<TinHuu_Addresses> TinHuu_Addresses { get; set; }
        public virtual DbSet<TinHuu_AssignmentHistory> TinHuu_AssignmentHistory { get; set; }
        public virtual DbSet<TinHuu_ChucVu> TinHuu_ChucVu { get; set; }
        public virtual DbSet<TinHuu_Conduct> TinHuu_Conduct { get; set; }
        public virtual DbSet<ToThamVieng> ToThamViengs { get; set; }
        public virtual DbSet<ToThamVieng_HoGiaDinh> ToThamVieng_HoGiaDinh { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VanThu> VanThus { get; set; }
        public virtual DbSet<CleryChucVuTemp> CleryChucVuTemps { get; set; }
        public virtual DbSet<GetBirthDayIn7Day> GetBirthDayIn7Day { get; set; }
        public virtual DbSet<v_assign> v_assign { get; set; }
        public virtual DbSet<v_church> v_church { get; set; }
        //public virtual DbSet<V_GetAllEndDay> V_GetAllEndDay { get; set; }
        public virtual DbSet<V_GetBirthIn7Day> V_GetBirthIn7Day { get; set; }
        public virtual DbSet<V_GetEndDay> V_GetEndDay { get; set; }
        public virtual DbSet<V_GetVeHuu> V_GetVeHuu { get; set; }
        public virtual DbSet<V_GroupGiaoPham> V_GroupGiaoPham { get; set; }
        public virtual DbSet<V_GroupGiaoPhamByChucDanh> V_GroupGiaoPhamByChucDanh { get; set; }
        public virtual DbSet<V_GroupGiaoPhamByChucDanhNotDuongChuc> V_GroupGiaoPhamByChucDanhNotDuongChuc { get; set; }
        public virtual DbSet<V_GroupHoiThanhByCapChiHoi> V_GroupHoiThanhByCapChiHoi { get; set; }
        public virtual DbSet<v_updateTenNhiemSo> v_updateTenNhiemSo { get; set; }
        public virtual DbSet<V_UserRoles> V_UserRoles { get; set; }
        public virtual DbSet<v_vanThu> v_vanThu { get; set; }
        public virtual DbSet<view_User> view_User { get; set; }
        public virtual DbSet<viewChurch> viewChurches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasMany(e => e.Clergies)
                .WithOptional(e => e.Address)
                .HasForeignKey(e => e.PermanentAddressId);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Clergies1)
                .WithOptional(e => e.Address1)
                .HasForeignKey(e => e.CurrentAddressId);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Clergies2)
                .WithOptional(e => e.Address2)
                .HasForeignKey(e => e.BirthAddressId);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Clergies3)
                .WithOptional(e => e.Address3)
                .HasForeignKey(e => e.ContactAddressId);

            modelBuilder.Entity<CapChiHoi>()
                .HasMany(e => e.Church_Assignment_History)
                .WithOptional(e => e.CapChiHoi)
                .HasForeignKey(e => e.CapChiHoi_Id);

            modelBuilder.Entity<CapChiHoi>()
                .HasMany(e => e.Clergy_AssignmentHistory)
                .WithOptional(e => e.CapChiHoi1)
                .HasForeignKey(e => e.CapChiHoi);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.CategoryDetails)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.CatId);

            modelBuilder.Entity<ChiTietChucVuGiaoPham>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<ChucVu>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<ChucVu>()
                .HasMany(e => e.ChiTietChucVuGiaoPhams)
                .WithRequired(e => e.ChucVu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Church>()
                .Property(e => e.ChurchName)
                .IsFixedLength();

            modelBuilder.Entity<Church>()
                .HasMany(e => e.Church_Assignment_History)
                .WithOptional(e => e.Church)
                .HasForeignKey(e => e.Church_Id);

            modelBuilder.Entity<Church>()
                .HasMany(e => e.Church_SoLuongTinHuu)
                .WithOptional(e => e.Church)
                .HasForeignKey(e => e.Church_Id);

            modelBuilder.Entity<Church>()
                .HasMany(e => e.Church_TAISAN)
                .WithOptional(e => e.Church)
                .HasForeignKey(e => e.Church_ID);

            modelBuilder.Entity<Church>()
                .HasMany(e => e.Church_TinHuu)
                .WithOptional(e => e.Church)
                .HasForeignKey(e => e.Church_Id);

            modelBuilder.Entity<Church>()
                .HasMany(e => e.Clergies)
                .WithOptional(e => e.Church)
                .HasForeignKey(e => e.CurrentChurchId);

            modelBuilder.Entity<Church_TinHuu>()
                .HasMany(e => e.Church_TinHuu_DanToc)
                .WithOptional(e => e.Church_TinHuu)
                .HasForeignKey(e => e.Church_TinHuu_Id);

            modelBuilder.Entity<City>()
                .HasMany(e => e.Churches)
                .WithOptional(e => e.City)
                .HasForeignKey(e => e.CitiId);

            modelBuilder.Entity<City>()
                .HasMany(e => e.Clergies)
                .WithOptional(e => e.City)
                .HasForeignKey(e => e.IDApprovedPlace);

            modelBuilder.Entity<Clergy>()
                .HasMany(e => e.ChiTietChucVuGiaoPhams)
                .WithRequired(e => e.Clergy)
                .HasForeignKey(e => e.GiaoPhamId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Clergy>()
                .HasMany(e => e.Clergy_AssignmentHistory)
                .WithRequired(e => e.Clergy)
                .HasForeignKey(e => e.ClergyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Clergy>()
                .HasMany(e => e.Clergy_Conduct)
                .WithRequired(e => e.Clergy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Clergy>()
                .HasMany(e => e.Clergy_Education)
                .WithRequired(e => e.Clergy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Clergy>()
                .HasMany(e => e.QuanHeVoiGiaoPhams)
                .WithOptional(e => e.Clergy)
                .HasForeignKey(e => e.ClergyId);

            modelBuilder.Entity<Clergy>()
                .HasMany(e => e.QuanHeVoiGiaoPhams1)
                .WithOptional(e => e.Clergy1)
                .HasForeignKey(e => e.ClergyId);

            modelBuilder.Entity<Clergy>()
                .HasMany(e => e.Clergy_TitleHistory)
                .WithOptional(e => e.Clergy)
                .HasForeignKey(e => e.CleargyId);

            modelBuilder.Entity<Clergy_AssignmentHistory>()
                .Property(e => e.Role)
                .IsFixedLength();

            modelBuilder.Entity<Clergy_AssignmentHistory>()
                .HasMany(e => e.Clergies)
                .WithOptional(e => e.Clergy_AssignmentHistory1)
                .HasForeignKey(e => e.CurrentAssignment_Id);

            modelBuilder.Entity<Clergy_TitleHistory>()
                .Property(e => e.RequestPaper)
                .IsFixedLength();

            modelBuilder.Entity<ClergyTitle>()
                .Property(e => e.Short)
                .IsFixedLength();

            modelBuilder.Entity<ClergyTitle>()
                .HasMany(e => e.Clergies)
                .WithOptional(e => e.ClergyTitle)
                .HasForeignKey(e => e.TitleId);

            modelBuilder.Entity<ClergyTitle>()
                .HasMany(e => e.Clergy_AssignmentHistory)
                .WithOptional(e => e.ClergyTitle)
                .HasForeignKey(e => e.TitleId);

            modelBuilder.Entity<ClergyTitle>()
                .HasMany(e => e.Clergy_TitleHistory)
                .WithRequired(e => e.ClergyTitle)
                .HasForeignKey(e => e.TitleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Conduct_Type>()
                .HasMany(e => e.Clergy_Conduct)
                .WithOptional(e => e.Conduct_Type)
                .HasForeignKey(e => e.ConductTypeId);

            modelBuilder.Entity<DanToc>()
                .HasMany(e => e.Church_TinHuu_DanToc)
                .WithOptional(e => e.DanToc)
                .HasForeignKey(e => e.DanToc_Id);

            modelBuilder.Entity<DanToc>()
                .HasMany(e => e.Clergies)
                .WithOptional(e => e.DanToc1)
                .HasForeignKey(e => e.DanToc);

            modelBuilder.Entity<HoGiaDinh>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<HoGiaDinh>()
                .HasMany(e => e.HoGiaDinhTinHuu)
                .WithOptional(e => e.HoGiaDinh)
                .HasForeignKey(e => e.HoGiaDinh_Id);

            modelBuilder.Entity<HoGiaDinh>()
                .HasMany(e => e.ToThamViengHoGiaDinh)
                .WithOptional(e => e.HoGiaDinh)
                .HasForeignKey(e => e.HoGiaDinh_Id);

            modelBuilder.Entity<HoGiaDinhTinHuu>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<PhanLoaiVanThu>()
                .HasMany(e => e.PhanLoaiVanThu1)
                .WithOptional(e => e.PhanLoaiVanThu2)
                .HasForeignKey(e => e.CapVanThu);

            modelBuilder.Entity<QuanHeGiaDinh>()
                .HasMany(e => e.QuanHeVoiGiaoPhams)
                .WithOptional(e => e.QuanHeGiaDinh)
                .HasForeignKey(e => e.RelationShipId);

            modelBuilder.Entity<QuanHeVoiGiaoPham>()
                .HasMany(e => e.BangCapNguoiPhoiNgaus)
                .WithOptional(e => e.QuanHeVoiGiaoPham)
                .HasForeignKey(e => e.QuanHeVoiGiaoPham_Id);

            modelBuilder.Entity<RoleMapping>()
                .HasMany(e => e.RoleDetails)
                .WithRequired(e => e.RoleMapping1)
                .HasForeignKey(e => e.RoleMapping)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TinHuu>()
                .HasMany(e => e.HoGiaDinhs)
                .WithOptional(e => e.TinHuu)
                .HasForeignKey(e => e.HomeOwner_Id);

            modelBuilder.Entity<TinHuu>()
                .HasMany(e => e.HoGiaDinh_TinHuu)
                .WithOptional(e => e.TinHuu)
                .HasForeignKey(e => e.TinHuu_Id);

            modelBuilder.Entity<TinHuu>()
                .HasMany(e => e.TinHuu_AssignmentHistory)
                .WithRequired(e => e.TinHuu)
                .HasForeignKey(e => e.TinHuu_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TinHuu>()
                .HasMany(e => e.TinHuu_Conduct)
                .WithRequired(e => e.TinHuu)
                .HasForeignKey(e => e.TinHuu_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TinHuu_AssignmentHistory>()
                .Property(e => e.Role)
                .IsFixedLength();

            modelBuilder.Entity<TinHuu_ChucVu>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<ToThamVieng>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<ToThamVieng>()
                .HasMany(e => e.ToThamVieng_HoGiaDinh)
                .WithOptional(e => e.ToThamVieng)
                .HasForeignKey(e => e.ToThamVieng_Id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.VanThus)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.DuocDuyetBoi);

            modelBuilder.Entity<GetBirthDayIn7Day>()
                .Property(e => e.Tuoi)
                .HasPrecision(18, 0);

            modelBuilder.Entity<v_assign>()
                .Property(e => e.ChurchName)
                .IsFixedLength();

            modelBuilder.Entity<v_church>()
                .Property(e => e.ChurchName)
                .IsFixedLength();

            modelBuilder.Entity<V_GetBirthIn7Day>()
                .Property(e => e.Tuoi)
                .HasPrecision(18, 0);

            modelBuilder.Entity<V_GetVeHuu>()
                .Property(e => e.Tuoi)
                .HasPrecision(18, 0);

            modelBuilder.Entity<V_GroupGiaoPham>()
                .Property(e => e.Short)
                .IsFixedLength();

            modelBuilder.Entity<V_GroupGiaoPhamByChucDanh>()
                .Property(e => e.Short)
                .IsFixedLength();

            modelBuilder.Entity<V_GroupGiaoPhamByChucDanhNotDuongChuc>()
                .Property(e => e.Short)
                .IsFixedLength();

            modelBuilder.Entity<v_updateTenNhiemSo>()
                .Property(e => e.ChurchName)
                .IsFixedLength();

            modelBuilder.Entity<viewChurch>()
                .Property(e => e.ChurchName)
                .IsFixedLength();
        }

        internal List<TimKiemVoiNhieuThamSo_Result> TimKiemVoiNhieuThamSo(string name, int? chucDanh, int? chucVu, int? capchihoi)
        {
            if (string.IsNullOrEmpty(name)) name = " ";
            if (chucDanh == null) chucDanh = 0;
            if (chucVu == null) chucVu = 0;
            if (capchihoi == null) capchihoi = 0;
                     
            var hovatenParameter = new SqlParameter("hovaten", name);
            var chucDanhParameter = new SqlParameter("ChucDanh", chucDanh);
            var chucVuParameter = new SqlParameter("ChucVu", chucVu) ;
            var capchihoiParameter = new SqlParameter("capchihoi", capchihoi) ;
            var result  = base.Database.SqlQuery<TimKiemVoiNhieuThamSo_Result>("TimKiemVoiNhieuThamSo @hovaten, @ChucDanh, @ChucVu, @capchihoi", hovatenParameter, chucDanhParameter, chucVuParameter, capchihoiParameter).ToList();
            return result;
        }

        public List<TimKiemGiaoPhamNangCao_Result> TimKiemGiaoPhamNangCao(string hovaten, int kethon, int dantoc, int quequan, int nhiemso, int tunamphucvu, int dennamphucvu, int cachtinh, int chucdanh, int chucvu)
        {
            var hovatenParameter = new SqlParameter("hovaten", hovaten);
            var kethonParameter = new SqlParameter("kethon", kethon);
            var dantocParameter = new SqlParameter("dantoc", dantoc);
            var quequanParameter = new SqlParameter("quequan", quequan);
            var nhiemsoParameter = new SqlParameter("nhiemso", nhiemso);
            var tunamphucvuParameter = new SqlParameter("tunamphucvu", tunamphucvu);
            var dennamphucvuParameter = new SqlParameter("dennamphucvu", dennamphucvu);
            var cachtinhParameter = new SqlParameter("cachtinh", cachtinh);
            var chucdanhParameter = new SqlParameter("chucdanh", chucdanh);
            var chucvuParameter = new SqlParameter("chucvu", chucvu);
            var result = base.Database.SqlQuery<TimKiemGiaoPhamNangCao_Result>("TimKiemGiaoPhamNangCao", hovatenParameter, kethonParameter, dantocParameter, quequanParameter,nhiemsoParameter,tunamphucvu,dennamphucvu,cachtinhParameter, chucdanhParameter,chucvuParameter, tunamphucvuParameter, dennamphucvuParameter).ToList();
            return result;
        }

        public List<TimKiemNangCao_Result> TimKiemNangCao(int? tinhthanh, int? chucvutu, int? chucvuden, int? nhiemkytu, int? nhiemkyden, int? namsinhtu, int? namsinhden, int? tinhtrang, int? chucdanh, int? chucvu, int? nhiemso, int? capnhiemso, int? dantoc, int? hocvi, string cmnd)
        {
            if (string.IsNullOrEmpty(cmnd)) { cmnd = " ";}
            if (tinhthanh == null) { tinhthanh = 0;}
            if (chucvutu == null) { chucvutu = 0;}
            if (chucvuden == null) { chucvuden = 0;}
            if (nhiemkytu == null) { nhiemkytu = 0;}
            if (namsinhtu == null) { namsinhtu = 0;}
            if (namsinhden == null) { namsinhden = 0;}
            if (tinhtrang == null) { tinhtrang = 0;}
            if (chucdanh == null) { chucdanh = 0;}
            if (chucvu == null) { chucvu = 0;}
            if (nhiemso == null) { nhiemso = 0;}
            if (capnhiemso == null) { capnhiemso = 0;}
            if (dantoc == null) { dantoc = 0;}
            if (hocvi == null) { hocvi = 0;}
            var tinhthanhParameter = new SqlParameter("tinhthanh", tinhthanh);
            var chucvutuParameter = new SqlParameter("chucvutu", chucvutu);
            var chucvudenParameter = new SqlParameter("chucvuden", chucvuden);
            var qnhiemkytuParameter = new SqlParameter("nhiemkytu", nhiemkytu);
            var nhiemkydenParameter = new SqlParameter("nhiemkyden", nhiemkyden);
            var namsinhtuParameter = new SqlParameter("namsinhtu", namsinhtu);
            var namsinhdenParameter = new SqlParameter("namsinhden", namsinhden);
            var tinhtrangParameter = new SqlParameter("tinhtrang", tinhtrang);
            var chucdanhParameter = new SqlParameter("chucdanh", chucdanh);
            var chucvuParameter = new SqlParameter("chucvu", chucvu);
            var nhiemsoParameter = new SqlParameter("nhiemso", nhiemso);
            var capnhiemsoParameter = new SqlParameter("capnhiemso", capnhiemso);
            var dantocParameter = new SqlParameter("dantoc", dantoc);
            var hocviParameter = new SqlParameter("hocvi", hocvi);
            var cmndParameter = new SqlParameter("cmnd", cmnd) ;
            var result = base.Database.SqlQuery<TimKiemNangCao_Result>("TimKiemNangCao  @tinhthanh, @chucvutu, @chucvuden, @nhiemkytu ,@nhiemkyden,@namsinhtu,@namsinhden,@tinhtrang,@chucdanh,@chucvu,@nhiemso,@capnhiemso,@dantoc, @hocvi , @cmnd", tinhthanhParameter, chucvutuParameter, chucvudenParameter, qnhiemkytuParameter, nhiemkydenParameter, namsinhtuParameter, namsinhdenParameter, tinhtrangParameter, chucdanhParameter, chucvuParameter, nhiemsoParameter, capnhiemsoParameter, dantocParameter, hocviParameter, cmndParameter).ToList();
            return result;
        }

        public List<GiaoPhamThoiGianPhucVu_Result> GiaoPhamThoiGianPhucVu(string tukhoa, int chucvu, int chucdanh, int coquan, int tunam, int dennam, int cachtinh)
        {
            var tukhoaParameter = new SqlParameter("tukhoa", tukhoa);
            var chucvuParameter = new SqlParameter("chucvu", chucvu);
            var chucdanhParameter = new SqlParameter("chucdanh", chucdanh);
            var coquanParameter = new SqlParameter("coquan", coquan);
            var tunamParameter = new SqlParameter("tunam", tunam);
            var dennamParameter = new SqlParameter("dennam", dennam);
            var cachtinhParameter = new SqlParameter("cachtinh", cachtinh);
            
            var result = base.Database.SqlQuery<GiaoPhamThoiGianPhucVu_Result>("GiaoPhamThoiGianPhucVu", tukhoaParameter,  chucdanhParameter, chucvuParameter, coquanParameter, tunamParameter, dennamParameter, cachtinhParameter).ToList();
            return result;
        }

        public List<GetPermissionList_Result> GetPermissionList()
        {
            
            var result = base.Database.SqlQuery<GetPermissionList_Result>("GetPermissionList").ToList();
            return result;
        }

        public List<GetCleryById_Result> GetCleryById(int id)
        {
            var _Id = new SqlParameter("@Id", id) ;
            var _test = new SqlParameter("@Test", "123");
            var result = base.Database.SqlQuery<GetCleryById_Result>("GetCleryById  @Id, @Test" , _Id,_test).ToList();
            return result;
        }

        public List<TimKiemGiaoPhamHetHan_Result> TimKiemGiaoPhamHetHan(string name, DateTime fromdate, int chucdanh, DateTime todate)
        {
            var nameParameter = new SqlParameter("name", name);
            var fromdateParameter = new SqlParameter("fromdate", fromdate);
            var chucdanhParameter = new SqlParameter("chucdanh", chucdanh);
            var todateParameter = new SqlParameter("todate", todate);
            var result = base.Database.SqlQuery<TimKiemGiaoPhamHetHan_Result>("TimKiemGiaoPhamHetHan",nameParameter,fromdateParameter,todateParameter,chucdanhParameter).ToList();
            return result;
        }
    }
}
