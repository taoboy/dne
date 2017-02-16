/**  
* file: JsonModel.cs
* Type: ORM Entity Class 
* Genreated date: 2016/8/1 11:22:16
* Host: kspc
* 
*┌───────────────────────────┐
*│　Copyright (c) 2015 Ks_Studio. All rights reserved.	 │
*└───────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.gdce_task.Model
{
    public class JsonModel
    {
        public JsonModel(bool success, String message, Object data)
        {
            this.success = success;
            this.message = message;
            this.data = data;
        }

        public bool success
        {
            get;
            set;
        }

        public String message
        {
            get;
            set;
        }

        public Object data
        {
            get;
            set;
        }
    }
}