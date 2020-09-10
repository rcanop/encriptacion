using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace Encriptacion.wpf.Domain
{
    [DataContract]
    public class UrlInfo : INotifyPropertyChanged
	{
		private string url;

		public string Sitio
		{
			get { return url; }
			set
			{
				if ((url is null && !(value is null)) || (value is null && !(url is null)) || url != value)
				{
					url = value;
					OnPropertyChanged(nameof(Sitio));
				}
			}
		}

		private string usr;

		public string Usuario
		{
			get { return usr; }
			set
			{
				if ((usr is null && !(value is null)) || (value is null && !(usr is null)) || usr != value)
				{
					usr = value;
					OnPropertyChanged(nameof(Usuario));
				}
			}
		}

		private string pwd;

		public string  Clave
		{
			get { return pwd; }
			set
			{
				if ((pwd is null && !(value is null)) || (value is null && !(pwd is null)) || pwd != value)
				{
					pwd = value;
					OnPropertyChanged(nameof(Clave));
				}
			}
		}

		private string recuperacion;

		public string Recuperacion
		{
			get { return recuperacion; }
			set
			{
				if ((recuperacion is null && !(value is null)) || (value is null && !(recuperacion is null)) || recuperacion != value)
				{
					recuperacion = value;
					OnPropertyChanged(nameof(Recuperacion));
				}
			}
		}

		private string claveWifi;

		public string ClaveWifi
		{
			get { return claveWifi; }
			set
			{
				if ((claveWifi is null && !(value is null)) || (value is null && !(claveWifi is null)) || claveWifi != value)
				{
					claveWifi = value;
					OnPropertyChanged(nameof(ClaveWifi));
				}
			}
		}

		private string wifi;


        public string Wifi
		{
			get { return wifi; }
			set {
				if ((wifi is null && !(value is null)) || (value is null && !(wifi is null)) || wifi != value)
				{
					wifi = value;
					OnPropertyChanged(nameof(Wifi));
				}
			}
		}
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string info = "")
        {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        #endregion
        public void GetData(UrlInfo data)
        {
			Sitio = data.Sitio;
			Usuario = data.Usuario;
			Clave = data.Clave;
			Recuperacion = data.Recuperacion;
			Wifi = data.Wifi;
			ClaveWifi = data.ClaveWifi;
        }

    }
}
