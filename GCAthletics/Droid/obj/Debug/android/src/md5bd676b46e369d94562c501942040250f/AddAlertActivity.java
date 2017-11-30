package md5bd676b46e369d94562c501942040250f;


public class AddAlertActivity
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
		mono.android.Runtime.register ("GCAthletics.Droid.AddAlertActivity, GCAthletics.Droid, Version=1.0.6542.42955, Culture=neutral, PublicKeyToken=null", AddAlertActivity.class, __md_methods);
	}


	public AddAlertActivity ()
	{
		super ();
		if (getClass () == AddAlertActivity.class)
			mono.android.TypeManager.Activate ("GCAthletics.Droid.AddAlertActivity, GCAthletics.Droid, Version=1.0.6542.42955, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
