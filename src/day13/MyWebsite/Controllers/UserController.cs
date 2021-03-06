using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Models;

namespace MyWebsite.Controllers
{
    [Route("api/[controller]s")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private static List<UserModel> _users = new List<UserModel>();


        /// <summary>
        /// 查询使用者清单
        /// </summary>
        /// <param name="q">查询使用者名称</param>
        /// <returns>使用者清单</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResultModel<IEnumerable<UserModel>>), 200)]
        [ProducesResponseType(typeof(ResultModel<string>), 500)]
        public ResultModel<IEnumerable<UserModel>> Get(string q)
        {
            var result = new ResultModel<IEnumerable<UserModel>>();
            result.Data = _users.Where(c => string.IsNullOrEmpty(q)
                                         || Regex.IsMatch(c.Name, q, RegexOptions.IgnoreCase));
            result.IsSuccess = true;
            return result;
        }

        [HttpGet("{id}")]
        public ResultModel Get(int id)
        {
            var result = new ResultModel();
            result.Data = _users.SingleOrDefault(c => c.Id == id);
            result.IsSuccess = true;
            return result;
        }

        [HttpPost]
        public ResultModel Post([FromBody]UserModel user)
        {
            var result = new ResultModel();
            user.Id = _users.Count() == 0 ? 1 : _users.Max(c => c.Id) + 1;
            _users.Add(user);
            result.Data = user.Id;
            result.IsSuccess = true;
            return result;
        }

        [HttpPut("{id}")]
        public ResultModel Put(int id, [FromBody]UserModel user)
        {
            var result = new ResultModel();
            int index;
            if ((index = _users.FindIndex(c => c.Id == id)) != -1)
            {
                _users[index] = user;
                result.IsSuccess = true;
            }
            return result;
        }

        [HttpDelete("{id}")]
        public ResultModel Delete(int id)
        {
            var result = new ResultModel();
            int index;
            if ((index = _users.FindIndex(c => c.Id == id)) != -1)
            {
                _users.RemoveAt(index);
                result.IsSuccess = true;
            }
            return result;
        }
    }
}