using System.Drawing;


#region << �� �� ע �� >>
/*----------------------------------------------------------------
// Copyright (C) 2017 �������ǿƼ����޹�˾ ��Ȩ���С� 
// ��Դ�汾������޸��˼����о�ʹ�ã�δ�����������Ͻ����á��������ǿƼ����޹�˾��һ�������Զ�����ҵ��Ӫ��������������˾����˾�н�OA�����ء���̬��΢��С����ȿ�����
// ���ڱ�ϵͳ����ذ�Ȩ�����������ǿƼ����У������ϵͳʹ�õ�������Դģ�飬��ģ���Ȩ����ԭ�������С�
// �����������ߵ��Ͷ��ɹ�����ͬ�ٽ���ҵ������չ��
// ��ؼ�������Ⱥ89226196 ,����QQ:249250126 ����΢��18695221159 ����:my820403@126.com
// �����ߣ�����
//----------------------------------------------------------------*/
#endregion
namespace Scada.Controls.Controls
{
	public class AuxiliaryLable
	{
		public string Text
		{
			get;
			set;
		}

		public Brush TextBrush
		{
			get;
			set;
		}

		public Brush TextBack
		{
			get;
			set;
		}

		public float LocationX
		{
			get;
			set;
		}

		public AuxiliaryLable()
		{
			TextBrush = Brushes.Black;
			TextBack = Brushes.Transparent;
			LocationX = 0.5f;
		}
	}
}
