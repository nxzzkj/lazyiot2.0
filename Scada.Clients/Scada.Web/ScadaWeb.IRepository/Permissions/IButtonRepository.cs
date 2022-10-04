using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScadaWeb.Common;
using ScadaWeb.Model;

namespace ScadaWeb.IRepository
{
    public interface IButtonRepository : IBaseRepository<ButtonModel>
    {
        /// <summary>
        /// 根据角色菜单按钮位置获得按钮列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="moduleId"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        IEnumerable<ButtonModel> GetButtonListByRoleIdModuleId(long roleId, long moduleId, PositionEnum position);
        IEnumerable<ButtonModel> GetButtonListByRoleIdModuleId(long roleId, long moduleId);
        /// <summary>
        /// 根据角色菜单获得按钮列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="moduleId"></param>
        /// <param name="selectList"></param>
        /// <returns></returns>
        IEnumerable<ButtonModel> GetButtonListByRoleIdModuleId(long roleId, long moduleId, out IEnumerable<ButtonModel> selectList);
    }
}
