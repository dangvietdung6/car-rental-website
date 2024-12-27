using System.Linq;
using System.Web.Mvc;
using ThueXeMay.Models;

namespace ThueXeMay.Areas.Admin.Controllers
{
    public class GpsTrackingController : BaseController
    {
        private readonly RENT_MOTOREntities db = new RENT_MOTOREntities();

        public ActionResult Index()
        {
            // Lấy dữ liệu GPS từ cơ sở dữ liệu
            var gpsData = db.GpsDatas // Lưu ý tên DbSet có thể là GpsDatas hoặc khác
                            .OrderByDescending(g => g.Timestamp)
                            .Take(10)
                            .ToList();

            return View(gpsData);
        }
    }
}
