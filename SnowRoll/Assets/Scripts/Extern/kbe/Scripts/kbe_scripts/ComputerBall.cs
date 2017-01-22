using SDK.Lib;

namespace KBEngine
{
    public class ComputerBall : KBEngine.GameObject   
    {
		public ComputerBall()
		{
            mEntity_SDK = new SDK.Lib.ComputerBall();
            mEntity_SDK.setEntity_KBE(this);
        }
		
		public override void __init__()
		{
            this.mEntity_SDK.setThisId((uint)this.id);
            this.mEntity_SDK.init();

            float radius = (float)getDefinedProperty("radius");
            (this.mEntity_SDK as SDK.Lib.BeingEntity).setMass(radius);

            Ctx.mInstance.mLogSys.log(string.Format("ComputerBall::__init__, eid = {0}", this.id), LogTypeId.eLogSplitMergeEmit);
        }

		public override void onDestroy ()
		{
            if (null != mEntity_SDK)
            {
                this.mEntity_SDK.dispose();
                this.mEntity_SDK = null;
            }
        }

        public override void SetPosition(object old)
        {
            base.SetPosition(old);

            if (null != this.mEntity_SDK)
            {
                this.mEntity_SDK.setPos(this.position);
            }
        }

        public override void SetDirection(object old)
        {
            base.SetDirection(old);

            if (null != this.mEntity_SDK)
            {
                this.mEntity_SDK.setRotateEulerAngle(this.direction);
            }
        }

        public virtual void updatePlayer(float x, float y, float z, float yaw)
		{
	    	position.x = x;
	    	position.y = y;
	    	position.z = z;
			
	    	direction.z = yaw;
		}
		
		public override void onEnterWorld()
		{
			base.onEnterWorld();
		}

        public override void onLeaveWorld()
        {
            base.onLeaveWorld();

            if (null != this.mEntity_SDK)
            {
                this.mEntity_SDK.dispose();
                this.mEntity_SDK = null;
            }
        }

        public override void server_set_position(object old)
        {
            base.server_set_position(old);
            if (null != this.mEntity_SDK)
            {
                (this.mEntity_SDK as SDK.Lib.BeingEntity).setDestPos(this.position, false);
            }
        }

        public override void set_name(object old)
        {
            string name = getDefinedProperty("name") as string;
            (this.mEntity_SDK as SDK.Lib.BeingEntity).setName(name);

            Ctx.mInstance.mLogSys.log(string.Format("ComputerBall::set_name, name = {0}", name), LogTypeId.eLogSplitMergeEmit);
        }
    }
}