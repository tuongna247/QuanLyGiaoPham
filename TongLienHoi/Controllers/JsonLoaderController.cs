using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HTTLVN.QLTLH.Code;
using HTTLVN.QLTLH.Models;
using HTTLVN.QLTLH.Models.SPEntity;

//using Omu.AwesomeMvc;

namespace HTTLVN.QLTLH.Controllers
{
    public class JsonLoaderController : Controller
    {
        private CodeFirstTongLienHoiEntities _db = new CodeFirstTongLienHoiEntities();
        //
        // GET: /JsonLoader/

        public ActionResult GetCleryAddressesId(int? id)
        {
            return
                Json(
                    _db.Addresses.OrderByDescending(a => a.CityId).ToList().Select(
                        o => new Category(){Id = o.Id, Name = o.Street + " " + o.Ward + " " + o.District}));
        }

        public ActionResult GetAllcity(int? v)
        {
            var list = _db.Cities.OrderBy(a => a.Name).ToList();
            list.Insert(0, new City() { id = 0, Name = "Chọn Tỉnh/Thành" });
            return Json(list.Select(o => new Category() { Name = o.Name, Id = o.id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllTermType(string v)
        {
            var list = new List<Category>
            {
                new Category() {Name = "Năm",Id =3}
            };
            return Json(list.Select(o => new Category() { Name = o.Name, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetYearOfBaptism(int? v)
        {
            var list = new List<Category>();
            var yearNow = DateTime.Now.Year;
            for (int i = yearNow - 80; i <= yearNow; i++)
            {
                list.Add(new Category() { Id = i, Name = i.ToString() });
            }
            return Json(list.Select(o => new Category() { Name = o.Name, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCityWithAll(int? v)
        {
            var all = _db.Cities.OrderBy(a => a.id).ToList();
            all.Insert(0, new City() { id = 0, Name = "Tất cả" });
            return Json(all.Select(o => new Category() { Name = o.Name, Id = o.id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClergyWithAll(int? v)
        {
            var all = _db.Clergies.ToList();

            foreach (var clergy in all)
            {
                clergy.TenDayDuKhongDau = clergy.FirstName ?? "";
                if (!string.IsNullOrWhiteSpace(clergy.TenDayDuKhongDau))
                    clergy.TenDayDuKhongDau += " " + clergy.MiddleName ?? "";
                else
                {
                    clergy.TenDayDuKhongDau += clergy.MiddleName ?? "";
                }
                if (!string.IsNullOrWhiteSpace(clergy.TenDayDuKhongDau))
                {
                    clergy.TenDayDuKhongDau += " " + clergy.LastName;
                }
                else
                {
                    clergy.TenDayDuKhongDau += clergy.LastName ?? "";
                }
                clergy.TenDayDuKhongDau = clergy.TenDayDuKhongDau.Trim().Replace("  "," ").Replace("  ", " ");

            }
            all = all.OrderBy(a => a.FirstName).ThenBy(a => a.LastName).ToList();
            all.Insert(0, new Clergy() { Id = 0, TenDayDuKhongDau = "Tất cả" });
            return Json(all.Select(o => new Category() { Name = o.TenDayDuKhongDau, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChonTinhThanh(int? v)
        {
            var all = _db.Cities.OrderBy(a => a.id).ToList();
            all.Insert(0, new City() { id = 0, Name = "Chọn Tỉnh/Thành" });
            return Json(all.Select(o => new Category() { Name = o.Name, Id = o.id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllAddress(int? id)
        {
            return
                Json(
                    _db.Addresses.OrderByDescending(a => a.Id).ToList().Select(
                        o => new Category() { Name = CommonFunction.GetAddress(o), Id = o.Id }));
        }

        public ActionResult GetChucDanh(int? v)
        {
            return
                Json(
                    _db.ClergyTitles.OrderBy(a => a.Id).ToList().Select(
                        o => new Category() { Name = o.Title, Id = o.Id }),JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetChucDanhSelec(int? v)
        {
            var all = _db.ClergyTitles.OrderBy(a => a.Id).ToList();
            all.Insert(0, new ClergyTitle() { Id = 0, Title = "Chọn chức danh" });
            return Json(all.Select(o => new ClergyTitle() { Title = o.Title, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllChucDanh(int? v)
        {
            var all = _db.ClergyTitles.OrderBy(a => a.Id).ToList();
            all.Insert(0, new ClergyTitle() { Id = 0, Title = "Chọn chức danh" });
            return Json(all.Select(o => new Category() { Name = o.Title, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllCapChiHoi(int? v)
        {
            var all = _db.CapChiHois.OrderBy(a => a.Description).ToList();
            all.Insert(0, new CapChiHoi() { id = 0, Description = "Chọn cấp chi hội" });
            return Json(all.Select(o => new Category() { Name = o.Description, Id = o.id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChucDanhWithAll(int? id)
        {
            var all = _db.ClergyTitles.Where(a => a.IsDuongChuc == true && a.Status == 1).OrderBy(a => a.Id).ToList();
            all.Insert(0, new ClergyTitle() { Id = 0, Title = "Tất cả" });
            return Json(all.Select(o => new Category() { Name = o.Title, Id = o.Id }), JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult GetTinhTrang(int? id)
        {
            var all = new List<ClergyTitle>
            {
                new ClergyTitle() {Id = 0, Title = "Chọn tình trạng"},
                new ClergyTitle() {Id = 1, Title = "Sắp hết nhiệm kỳ"},
                new ClergyTitle() {Id = 2, Title = "Đã hết nhiệm kỳ"}
            };
            return Json(all.Select(o => new Category() { Name = o.Title, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTinhTrangHonNhan(int? id)
        {
            var all = new List<ClergyTitle>
            {
                new ClergyTitle() {Id = 0, Title = "Tất cả"},
                new ClergyTitle() {Id = 1, Title = "Kết hôn"},
                new ClergyTitle() {Id = 2, Title = "Độc thân"}
            };
            return Json(all.Select(o => new Category() { Name = o.Title, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChonHocVi(int? id)
        {
            var all = new List<ClergyTitle>
            {
                new ClergyTitle() {Id = 0, Title = "Chọn học vị"},
                new ClergyTitle() {Id = 1, Title = "Bổ túc"},
                new ClergyTitle() {Id = 2, Title = "Cử nhân"},
                new ClergyTitle() {Id = 3, Title = "Thạc sĩ"},
                new ClergyTitle() {Id = 4, Title = "Tiến sĩ"}
            };
            return Json(all.Select(o => new Category() { Name = o.Title, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDanToc(int? id)
        {
            var all = _db.DanTocs.ToList();
            all.Insert(0, new DanToc() { Id = 0, Name = "Tất cả" });
            return Json(all.Select(o => new Category() { Name = o.Name, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChonDanToc(int? id)
        {
            var all = _db.DanTocs.ToList();
            all.Insert(0, new DanToc() { Id = 0, Name = "Chọn dân tộc" });
            return Json(all.Select(o => new Category() { Name = o.Name, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChonDanTocs(int? v)
        {
            var all = _db.DanTocs.OrderBy(a => a.Name).ToList();
            all.Insert(0, new DanToc() { Id = 0, Name = "Chọn dân tộc" });
            return Json(all.Select(o => new Category() { Name = o.Name, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNamSinh(int? id)
        {
            var all = new List<Category> { new Category() { Id = 0, Name = "Tất cả" } };
            for (var from = DateTime.Now.Year - 80; from < DateTime.Now.Year - 20; from++)
            {
                all.Add(new Category() { Id = from, Name = from.ToString() });
            }
            return Json(all.Select(o => new Category() { Name = o.Name, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNamChucVu(int? id)
        {
            var all = new List<Category> { new Category() { Id = 0, Name = "Tất cả" } };
            for (var from = 50; from >= 0; from = from - 5)
            {
                all.Add(new Category() { Id = from, Name = from.ToString() });
            }
            return Json(all.Select(o => new Category() { Name = o.Name, Id = o.Id }), JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult GetChucVuWithAll(int? id)
        {
            var all = _db.ChucVus.OrderBy(a => a.id).ToList();
            all.Insert(0, new ChucVu() { id = 0, Name = "Tất cả" });
          
            return Json(all.Select(o=> new Category(){ Name =  o.Name, Id =  o.id} ), JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetChucVu(int? v)
        {
            var all = _db.ChucVus.OrderBy(a => a.id).ToList();
            all.Insert(0, new ChucVu() { id = 0, Name = "Chọn chức vụ" });
            return Json(all.Select(o => new Category() { Name = o.Name, Id = o.id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCapChiHoiWithAll(int? v)
        {
            var all = _db.CapChiHois.Where(a => a.Status == 1).OrderBy(a => a.sortId).ToList();

            all.Insert(0, new CapChiHoi() { id = 0, Description = "Chọn cấp nhiệm sở" });
            return Json(all.Select(o => new Category() { Name = o.Description, Id = o.id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCapChiHoiGiaoSo(int? id)
        {
            var all = _db.CapChiHois.Where(a => a.Status == 1).OrderBy(a => a.sortId).ToList();

            all.Insert(0, new CapChiHoi() { id = 0, Description = "Chọn cấp nhiệm sở" });
            return Json(all.Select(o => new Category() { Name = o.Description, Id = o.id }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCapNhiemSo(int? v)
        {
            var all = _db.CapChiHois.OrderBy(a => a.id).ToList();
            all.Insert(0, new CapChiHoi() { id = 0, Description = "Chọn cấp nhiệm sở" });
            return Json(all.Select(o => new Category() { Name = o.Description, Id = o.id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDanTocWithAll(int? id)
        {
            var all = _db.DanTocs.OrderBy(a => a.Id).ToList();
            all.Insert(0, new DanToc() { Id = 0, Description = "Tất cả" });
            return Json(all.Select(o => new Category() { Name = o.Description, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHoiThanh(int? v)
        {
            var all = _db.Churches.OrderBy(a => a.ChurchName).ToList();
            all.Insert(0, new Church() { Id = 0, ChurchName = "Chọn Nhiệm sở" });
            return Json(all.Select(o => new Category() { Name = o.ChurchName, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVanPhong(int? v)
        {
            var all = _db.Offices.OrderBy(a => a.Id).ToList();
            all.Insert(0, new Office() { Id = 0, Description = " ", OfficeName = " " });
            return Json(all.Select(o => new Category() { Name = o.Description, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetThoiHan(int? v)
        {
            var all = new Dictionary<int, int> { { 2, 2 }, { 4, 4 } };
            return Json(all.Select(i => new Category() {Name = i.Key.ToString(), Id = i.Value}),
                JsonRequestBehavior.AllowGet);
            //(i.Key, i.Value + "", i.Key == v)));
        }

        public ActionResult GetQuanHeGiaDinh(int? id)
        {
            var all = _db.QuanHeGiaDinhs.ToList();

            return Json(all.Select(o => new Category() { Name = o.Name, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCachTinh(int? id)
        {
            var all = new List<Category> { new Category() { Id = 0, Name = "Từ lúc bắt đầu chức vụ" } };
            all.Add(new Category() { Id = 1, Name = "Từ lúc bắt đầu chức vụ" });
            return Json(all.Select(o => new Category() { Name = o.Name, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLoaiVanThu(int? v)
        {
            var all = new List<ClergyTitle>
             {
                 new ClergyTitle() {Id = 0, Title = "Chọn loại văn thư"},
                 new ClergyTitle() {Id = 1, Title = "Văn thư đến"},
                 new ClergyTitle() {Id = 2, Title = "Văn thư đi"}
             };
            return Json(all.Select(o => new Category() { Name = o.Title, Id = o.Id }), JsonRequestBehavior.AllowGet);
        }
    }
}
