using JFramework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace JFramework.Game
{
    /// <summary>
    /// 判断战斗是否结束
    /// </summary>
    public class JCombatQuery : DictionaryContainer<IJCombatTeam>, IJCombatQuery
    {
        IJCombatFrameRecorder frameRecorder;

        IJCombatTeam winner;

        IJCombatAttrNameQuery attrNameQuery;

        protected JCombat combat;

        protected ILogger logger;

        public JCombatQuery( Func<IJCombatTeam, string> keySelector, IJCombatFrameRecorder frameRecorder, IJCombatAttrNameQuery attrNameQuery) : base(keySelector)
        {
            this.frameRecorder = frameRecorder;
            this.attrNameQuery = attrNameQuery;
        }

        public JCombatQuery(IJCombatFrameRecorder frameRecorder, IJCombatAttrNameQuery attrNameQuery) : this((team) => team.Uid, frameRecorder, attrNameQuery)
        {
        }

        public JCombatQuery(List<IJCombatTeam> teams, Func<IJCombatTeam, string> keySelector, IJCombatFrameRecorder frameRecorder, IJCombatAttrNameQuery attrNameQuery) : this(keySelector, frameRecorder, attrNameQuery) 
        {
            SetTeams(teams);
        }

        public void SetTeams(List<IJCombatTeam> teams)
        {
            Clear();
            AddRange(teams);

            foreach (var team in teams)
            {
                team.SetQuery(this);
            }
        }

        #region 查找战斗结果
        /// <summary>
        /// 获取胜利者队伍
        /// </summary>
        /// <returns></returns>
        public IJCombatTeam GetWinner()
        {
            return winner;
        }

        /// <summary>
        /// 战斗是否结束：剩余1对活着的，回合达到上限结束
        /// </summary>
        /// <returns></returns>
        public virtual bool IsCombatOver()
        {
            winner = null;

            if (frameRecorder.IsMaxFrame())
                return true;

            var teams = GetAll();
            var aliveTeamCount = teams.Count;
            foreach (var team in teams)
            {
                var allDead = team.IsAllDead();
                if (!allDead)
                    winner = team;
                else
                    aliveTeamCount -= 1;
            }

            if (aliveTeamCount != 1)
                winner = null;

            return aliveTeamCount <= 1;
        }
        #endregion

        #region 查询帧
        public bool IsMaxFrame() => frameRecorder.IsMaxFrame();

        public int GetCurFrame() => frameRecorder.GetCurFrame();

        public int GetMaxFrame() => frameRecorder.GetMaxFrame();


        #endregion

        #region 查找战斗单位信息
        public IJCombatTeam GetTeam(string teamUid)
        {
            return Get(teamUid);
        }

        /// <summary>
        /// 根据unit查找所在队伍
        /// </summary>
        /// <param name="unitUid"></param>
        /// <returns></returns>
        public IJCombatTeam GetTeamByUnit(string unitUid)
        {
            var teams = GetTeams();
            foreach (var team in teams)
            {
                var unit = team.GetUnit(unitUid);
                if (unit != null)
                    return team;
            }

            return null;
        }

        /// <summary>
        /// 获取所有敌人队伍
        /// </summary>
        /// <param name="sourceTeam"></param>
        /// <returns></returns>
        public List<IJCombatTeam> GetOppoTeams(IJCombatTeam sourceTeam)
        {
            var result = new List<IJCombatTeam>();

            var teams = GetTeams();
            foreach (var team in teams)
            {
                if (_keySelector(team) == _keySelector(sourceTeam)) //to do: 定义是否是友军的查询接口 IJCombatFriendTeamQuery
                    continue;

                result.Add(team);
            }

            return result;
        }

        /// <summary>
        /// 获取所有敌人队伍
        /// </summary>
        /// <param name="unitUid"></param>
        /// <returns></returns>
        public List<IJCombatTeam> GetOppoTeamsByUnit(string unitUid)
        {
            var myTeam = GetTeamByUnit(unitUid);
            return GetOppoTeams(myTeam);
        }

        /// <summary>
        /// 获取所有队伍
        /// </summary>
        /// <returns></returns>
        public List<IJCombatTeam> GetTeams()
        {
            return GetAll();
        }

        public IJCombatUnit GetUnit(string unitUid)
        {
            var teams = GetTeams();
            foreach(var team in teams)
            {
                var unit = team.GetUnit(unitUid);
                if (unit != null)
                    return unit;

            }
            return null;
        }

        public List<IJCombatUnit> GetUnits()
        {
            var result = new List<IJCombatUnit>();

            foreach(var team in GetTeams())
            {
                result.AddRange(team.GetAllUnits());
            }

            return result;
        }

        public List<IJCombatUnit> GetUnits(string teamUid)
        {
            var team = Get(teamUid);    
            return team.GetAllUnits();   
        }

        public List<IJCombatUnit> GetUnits(Func<IJCombatUnit, bool> func)
        {
            var units = GetUnits();
            var result = new List<IJCombatUnit>();

            foreach (var item in units)
            {
                if(func(item))
                    result.Add(item);
            }

            return result;
        }

        public string GetHpAttrName() => attrNameQuery.GetHpAttrName();

        public string GetMaxHpAttrName() => attrNameQuery.GetMaxHpAttrName();

        public string GetAttrName(int nameId) => attrNameQuery.GetAttrName(nameId);

        #endregion

        public void SetCombat(JCombat combat)
        {
            this.combat = combat;
        }

        public JCombat GetCombat()
        {
            return combat;
        }

        public void SetLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public ILogger GetLogger()
        {
            return logger;
        }
    }
}
