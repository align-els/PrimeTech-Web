using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RecipeJungle
{
    public static class ActionUtils
    {
        public static IActionResult Success() {
            return Json(true, 200);
        }

        public static IActionResult Success<T>(T obj) {
            return Json(obj, 200);
        }

        public static IActionResult Error<T>(T obj) {
            return Json(obj, 420);
        }

        public static IActionResult Json<T>(T obj, int status) {
            return Json(JsonConvert.SerializeObject(obj), status);
        }

        public static IActionResult Json(string json, int status) {
            return new ContentResult {
                Content = json,
                ContentType = "application/json",
                StatusCode = status
            };
        }
    }
}
