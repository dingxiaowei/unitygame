using Game.UI;
using SDK.Common;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public class MaterialMgr : ResMgrBase
    {
        //public Dictionary<MaterialID, Material> m_ID2MatDic = new Dictionary<MaterialID, Material>();

        public MaterialMgr()
        {

        }

        // 通过职业获取卡牌组的材质
        public MatRes getCardGroupMatByOccup(EnPlayerCareer Occup)
        {
            string prefab = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupMatAttrDic[(int)Occup].m_prefabName;
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupMatAttrDic[(int)Occup].m_path;

            return syncGet<MatRes>(prefab, path) as MatRes;
        }

        // 通过职业获取卡牌组LOGO的材质
        public MatRes getCardGroupLOGOMatByOccup(EnPlayerCareer Occup)
        {
            string prefab = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupMatAttrDic[(int)Occup].m_logoPrefabName;
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupMatAttrDic[(int)Occup].m_logoPath;

            return syncGet<MatRes>(prefab, path) as MatRes;
        }

        public override void onLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
            string path = res.GetPath();

            // 获取资源单独保存
            (m_path2ResDic[path] as MatRes).m_mat = res.getObject(res.getPrefabName()) as Material;

            if (m_path2ListenItemDic[path].m_loaded != null)
            {
                m_path2ListenItemDic[path].m_loaded(m_path2ResDic[path]);
            }

            base.onLoaded(resEvt);
        }
    }
}