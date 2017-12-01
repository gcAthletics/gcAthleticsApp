package md518909d699eb3acb2630906a609e8c249;


public class AddEventActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("GCAthletics.Droid.AddEventActivity, GCAthletics.Droid, Version=1.0.6543.35180, Culture=neutral, PublicKeyToken=null", AddEventActivity.class, __md_methods);
	}


	public AddEventActivity ()
	{
		super ();
		if (getClass () == AddEventActivity.class)
			mono.android.TypeManager.Activate ("GCAthletics.Droid.AddEventActivity, GCAthletics.Droid, Version=1.0.6543.35180, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
