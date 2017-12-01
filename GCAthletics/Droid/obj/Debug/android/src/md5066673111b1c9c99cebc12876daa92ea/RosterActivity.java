package md5066673111b1c9c99cebc12876daa92ea;


public class RosterActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onBackPressed:()V:GetOnBackPressedHandler\n" +
			"";
		mono.android.Runtime.register ("GCAthletics.Droid.RosterActivity, GCAthletics.Droid, Version=1.0.6543.34593, Culture=neutral, PublicKeyToken=null", RosterActivity.class, __md_methods);
	}


	public RosterActivity ()
	{
		super ();
		if (getClass () == RosterActivity.class)
			mono.android.TypeManager.Activate ("GCAthletics.Droid.RosterActivity, GCAthletics.Droid, Version=1.0.6543.34593, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onBackPressed ()
	{
		n_onBackPressed ();
	}

	private native void n_onBackPressed ();

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
