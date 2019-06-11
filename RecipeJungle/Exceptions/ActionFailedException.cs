using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeJungle.Exceptions {

    public class ActionFailedException : Exception {

        public ActionFailedException(string message) : base(message) {

        }

        public ActionFailedException() : this("") {

        }
    }
}
