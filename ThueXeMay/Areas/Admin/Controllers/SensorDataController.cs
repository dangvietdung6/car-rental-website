using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThueXeMay.Helpers;
using ThueXeMay.Models;

namespace ThueXeMay.Areas.Admin.Controllers
{
    public class SensorController : BaseController
    {
        private readonly RENT_MOTOREntities db = new RENT_MOTOREntities();

        // Action hiển thị dữ liệu cảm biến
        public async Task<ActionResult> Index()
        {
            try
            {
                // Lấy dữ liệu từ Firebase
                var firebaseData = await FirebaseHelper.GetDataAsync<dynamic>("SensorData");

                if (firebaseData != null)
                {
                    // Lưu dữ liệu vào cơ sở dữ liệu
                    SensorData sensorData = new SensorData
                    {
                        Timestamp = DateTime.UtcNow,
                        X = firebaseData.X,
                        Y = firebaseData.Y,
                        Z = firebaseData.Z
                    };

                    db.SensorDatas.Add(sensorData);
                    db.SaveChanges();

                    // Chỉ giữ lại 5 lần gần nhất
                    var allData = db.SensorDatas.OrderByDescending(s => s.Timestamp).ToList();
                    if (allData.Count > 5)
                    {
                        var dataToRemove = allData.Skip(5).ToList();
                        db.SensorDatas.RemoveRange(dataToRemove);
                        db.SaveChanges();
                    }

                    // Lấy 5 dữ liệu mới nhất để hiển thị
                    var recentData = db.SensorDatas.OrderByDescending(s => s.Timestamp).Take(5).ToList();
                    ViewBag.SensorData = recentData;

                    // Phân tích dữ liệu và thêm thông báo
                    var statusMessage = AnalyzeSensorData(recentData);
                    ViewBag.StatusMessage = statusMessage;
                }
                else
                {
                    ViewBag.SensorData = null;
                    ViewBag.StatusMessage = "Không có dữ liệu cảm biến từ Firebase.";
                }
            }
            catch (Exception ex)
            {
                // Ghi lỗi và gửi thông báo lỗi ra View
                ViewBag.SensorData = null;
                ViewBag.StatusMessage = "Lỗi khi tải dữ liệu cảm biến: " + ex.Message;
            }

            return View();
        }

        // Phương thức phân tích dữ liệu cảm biến
        private string AnalyzeSensorData(System.Collections.Generic.List<SensorData> data)
        {
            if (data == null || data.Count < 2)
            {
                return "Dữ liệu không đủ để phân tích.";
            }

            var last = data.First();
            var previous = data.Last();

            var deltaX = Math.Abs(last.X - previous.X);
            var deltaY = Math.Abs(last.Y - previous.Y);
            var deltaZ = Math.Abs(last.Z - previous.Z);

            // Định nghĩa ngưỡng
            const double stableThreshold = 5.0;//dưới mức này là nghiêng ở giữa là ổn định
            const double slightTiltThreshold = 10.0;

            // Phân loại trạng thái
            if (deltaX < stableThreshold && deltaY < stableThreshold && deltaZ < stableThreshold)
            {
                return "Xe x đang ổn định.";
            }
            else if (deltaX < slightTiltThreshold && deltaY < slightTiltThreshold && deltaZ < slightTiltThreshold)
            {
                return "Xe x hơi nghiêng.";
            }
            else
            {
                return "Xe x đang nghiêng quá mức!";
            }
        }
    }
}




















//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web.Mvc;
//using ThueXeMay.Helpers;
//using ThueXeMay.Models;

//namespace ThueXeMay.Areas.Admin.Controllers
//{
//    public class SensorController : BaseController
//    {
//        private readonly RENT_MOTOREntities db = new RENT_MOTOREntities();

//        // Action hiển thị dữ liệu cảm biến
//        public async Task<ActionResult> Index()
//        {
//            try
//            {
//                // Lấy dữ liệu từ Firebase
//                var firebaseData = await FirebaseHelper.GetDataAsync<dynamic>("SensorData");

//                if (firebaseData != null)
//                {
//                    // Lưu dữ liệu vào cơ sở dữ liệu
//                    SensorData sensorData = new SensorData
//                    {
//                        Timestamp = DateTime.UtcNow,
//                        X = firebaseData.X,
//                        Y = firebaseData.Y,
//                        Z = firebaseData.Z
//                    };

//                    db.SensorDatas.Add(sensorData);
//                    db.SaveChanges();

//                    // Chỉ giữ lại 5 lần gần nhất
//                    var allData = db.SensorDatas.OrderByDescending(s => s.Timestamp).ToList();
//                    if (allData.Count > 5)
//                    {
//                        var dataToRemove = allData.Skip(5).ToList();
//                        db.SensorDatas.RemoveRange(dataToRemove);
//                        db.SaveChanges();
//                    }

//                    // Lấy 5 dữ liệu mới nhất để hiển thị
//                    var recentData = db.SensorDatas.OrderByDescending(s => s.Timestamp).Take(5).ToList();
//                    ViewBag.SensorData = recentData;

//                    // Phân tích dữ liệu và đưa ra thông báo
//                    var statusMessage = AnalyzeSensorData(recentData);
//                    ViewBag.StatusMessage = statusMessage;
//                }
//                else
//                {
//                    ViewBag.SensorData = null; // Không có dữ liệu
//                    ViewBag.StatusMessage = "Không có dữ liệu cảm biến.";
//                }

//                return View();
//            }
//            catch (Exception ex)
//            {
//                // Ghi log lỗi (nếu cần)
//                ViewBag.ErrorMessage = "Lỗi khi tải dữ liệu cảm biến: " + ex.Message;
//                return View();
//            }
//        }

//        // Phương thức phân tích dữ liệu cảm biến
//        private string AnalyzeSensorData(System.Collections.Generic.List<SensorData> data)
//        {
//            if (data == null || data.Count < 2)
//            {
//                return "Dữ liệu không đủ để phân tích.";
//            }

//            var last = data.First();
//            var previous = data.Last();

//            var deltaX = Math.Abs(last.X - previous.X);
//            var deltaY = Math.Abs(last.Y - previous.Y);
//            var deltaZ = Math.Abs(last.Z - previous.Z);

//            // Định nghĩa ngưỡng
//            const double stableThreshold = 5.0;
//            const double slightTiltThreshold = 10.0;

//            // Phân loại trạng thái
//            if (deltaX < stableThreshold && deltaY < stableThreshold && deltaZ < stableThreshold)
//            {
//                return "Hệ thống đang ổn định.";
//            }
//            else if (deltaX < slightTiltThreshold && deltaY < slightTiltThreshold && deltaZ < slightTiltThreshold)
//            {
//                return "Hệ thống hơi nghiêng.";
//            }
//            else
//            {
//                return "Hệ thống đang nghiêng quá mức!";
//            }
//        }
//    }
//}








































//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web.Mvc;
//using ThueXeMay.Helpers;
//using ThueXeMay.Models;

//namespace ThueXeMay.Areas.Admin.Controllers
//{
//    public class SensorController : BaseController
//    {
//        private readonly RENT_MOTOREntities db = new RENT_MOTOREntities();

//        // Action hiển thị dữ liệu cảm biến
//        public async Task<ActionResult> Index()
//        {
//            try
//            {
//                // Lấy dữ liệu từ Firebase
//                var firebaseData = await FirebaseHelper.GetDataAsync<dynamic>("SensorData");

//                if (firebaseData != null)
//                {
//                    // Lưu dữ liệu vào cơ sở dữ liệu
//                    SensorData sensorData = new SensorData
//                    {
//                        Timestamp = DateTime.UtcNow,
//                        X = firebaseData.X,
//                        Y = firebaseData.Y,
//                        Z = firebaseData.Z
//                    };

//                    db.SensorDatas.Add(sensorData);
//                    db.SaveChanges();

//                    // Chỉ giữ lại 5 lần gần nhất
//                    var allData = db.SensorDatas.OrderByDescending(s => s.Timestamp).ToList();
//                    if (allData.Count > 5)
//                    {
//                        var dataToRemove = allData.Skip(5).ToList();
//                        db.SensorDatas.RemoveRange(dataToRemove);
//                        db.SaveChanges();
//                    }

//                    // Lấy 5 dữ liệu mới nhất để hiển thị
//                    var recentData = db.SensorDatas.OrderByDescending(s => s.Timestamp).Take(5).ToList();
//                    ViewBag.SensorData = recentData;
//                }
//                else
//                {
//                    ViewBag.SensorData = null; // Không có dữ liệu
//                }

//                return View();
//            }
//            catch (Exception ex)
//            {
//                // Ghi log lỗi (nếu cần)
//                ViewBag.ErrorMessage = "Lỗi khi tải dữ liệu cảm biến: " + ex.Message;
//                return View();
//            }
//        }
//    }
//}

































//using System.Threading.Tasks;
//using System.Web.Mvc;
//using ThueXeMay.Helpers;

//namespace ThueXeMay.Areas.Admin.Controllers
//{
//    public class SensorController : BaseController
//    {
//        // Action hiển thị dữ liệu cảm biến
//        public async Task<ActionResult> Index()
//        {
//            // Lấy dữ liệu từ Firebase
//            var data = await FirebaseHelper.GetDataAsync<dynamic>("SensorData");
//            if (data == null)
//            {
//                ViewBag.SensorData = "Không có dữ liệu";
//            }
//            else
//            {
//                ViewBag.SensorData = data;
//            }// Đường dẫn trong Firebase
//            return View(); // Truyền dữ liệu cho view
//        }
//    }
//}
