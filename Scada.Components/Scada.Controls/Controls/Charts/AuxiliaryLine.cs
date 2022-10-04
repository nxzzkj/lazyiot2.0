

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
using System;
using System.Drawing;

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class AuxiliaryLine.
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
	internal class AuxiliaryLine : IDisposable
	{
		private bool disposedValue = false;

		public float Value
		{
			get;
			set;
		}

		public float PaintValue
		{
			get;
			set;
		}

		public float PaintValueBackUp
		{
			get;
			set;
		}

		public Color LineColor
		{
			get;
			set;
		}

		public Pen PenDash
		{
			get;
			set;
		}

		public Pen PenSolid
		{
			get;
			set;
		}

		public float LineThickness
		{
			get;
			set;
		}

		public Brush LineTextBrush
		{
			get;
			set;
		}

		public bool IsLeftFrame
		{
			get;
			set;
		}

        private bool isDashStyle = true;

        public bool IsDashStyle
        {
            get { return isDashStyle; }
            set { isDashStyle = value; }
        }		


		public Pen GetPen()
		{
			return IsDashStyle ? PenDash : PenSolid;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
                    if(PenDash==null)
					PenDash.Dispose();
                    if(PenSolid==null)
					PenSolid.Dispose();
                    if(LineTextBrush==null)
					LineTextBrush.Dispose();
				}
				disposedValue = true;
			}
		}

        public string Tip { get; set; }

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
