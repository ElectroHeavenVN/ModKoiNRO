public class Skill
{
	public const sbyte ATT_STAND = 0;

	public const sbyte ATT_FLY = 1;

	public const sbyte SKILL_AUTO_USE = 0;

	public const sbyte SKILL_CLICK_USE_ATTACK = 1;

	public const sbyte SKILL_CLICK_USE_BUFF = 2;

	public const sbyte SKILL_CLICK_NPC = 3;

	public const sbyte SKILL_CLICK_LIVE = 4;

	public SkillTemplate template;

	public short skillId;

	public int point;

	public long powRequire;

	public int coolDown;

	public long lastTimeUseThisSkill;

	public int dx;

	public int dy;

	public int maxFight;

	public int manaUse;

	public SkillOption[] options;

	public bool paintCanNotUseSkill;

	public short damage;

	public string moreInfo;

	public short price;

	public string strTimeReplay()
	{
		if (coolDown % 1000 == 0)
			return coolDown / 1000 + string.Empty;
		int num = coolDown % 1000;
		return coolDown / 1000 + "." + ((num % 100 != 0) ? (num / 10) : (num / 100));
	}

	public void paint(int x, int y, mGraphics g)
	{
		SmallImage.drawSmallImage(g, template.iconId, x, y, 0, StaticObj.VCENTER_HCENTER);
		long num = mSystem.currentTimeMillis() - lastTimeUseThisSkill;
		if (num >= coolDown)
		{
			paintCanNotUseSkill = false;
			return;
		}
		paintCanNotUseSkill = true;
		g.setColor(2721889, 0.7f);
		int num2 = (int)(num * 20L / coolDown);
		g.fillRect(x - 10, y - 10 + num2, 20, 20 - num2);
		long num3 = coolDown - num;
		if (num3 > 10000L)
			mFont.tahoma_7.drawString(g, NinjaUtil.getMoneys(num3).Split('.')[0], x, y - 6, 2);
		else if (num3 > 1000L)
		{
			mFont.tahoma_7.drawString(g, NinjaUtil.getMoneys(num3).Substring(0, 3), x, y - 6, 2);
		}
		else
		{
			mFont.tahoma_7.drawString(g, "0." + num3.ToString().Substring(0, 2), x, y - 6, 2);
		}
	}
}
