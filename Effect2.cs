public abstract class Effect2
{
	public static MyVector vEffect3;

	public static MyVector vEffect2;

	public static MyVector vRemoveEffect2;

	public static MyVector vEffect2Outside;

	public static MyVector vAnimateEffect;

	public static MyVector vEffectFeet;

	public virtual void update()
	{
	}

	public virtual void paint(mGraphics g)
	{
	}

	static Effect2()
	{
		vEffect3 = new MyVector();
		vEffect2 = new MyVector();
		vRemoveEffect2 = new MyVector();
		vEffect2Outside = new MyVector();
		vAnimateEffect = new MyVector();
		vEffectFeet = new MyVector();
	}
}
