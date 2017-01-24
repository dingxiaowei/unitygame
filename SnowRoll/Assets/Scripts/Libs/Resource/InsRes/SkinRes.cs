﻿using Mono.Xml;
using System.Collections;
using System.Security;

namespace SDK.Lib
{
    public class SkinRes : TextResBase
    {
        protected string[] mBoneArr;   // 蒙皮的骨头列表

        public string[] boneArr
        {
            get
            {
                return mBoneArr;
            }
            set
            {
                mBoneArr = value;
            }
        }

        override protected void initImpl(ResItem res)
        {
            base.initImpl(res);

            SecurityParser xmlDoc = new SecurityParser();
            xmlDoc.LoadXml(mText);

            SecurityElement rootNode = xmlDoc.ToXml();
            ArrayList itemMeshList = rootNode.Children;
            SecurityElement itemMesh;

            ArrayList itemSubMeshList;
            SecurityElement itemSubMesh;
            string meshName = "";
            string subMeshName = "";
            string bonesList = "";

            foreach (SecurityElement itemNode1f in itemMeshList)
            {
                itemMesh = itemNode1f;
                UtilXml.getXmlAttrStr(itemMesh, "name", ref meshName);

                itemSubMeshList = itemMesh.Children;
                foreach (SecurityElement itemNode2f in itemSubMeshList)
                {
                    itemSubMesh = itemNode2f;
                    UtilXml.getXmlAttrStr(itemSubMesh, "name", ref subMeshName);
                    UtilXml.getXmlAttrStr(itemSubMesh, "bonelist", ref bonesList);
                    mBoneArr = bonesList.Split(',');
                }
            }

            mText = "";
        }
    }
}