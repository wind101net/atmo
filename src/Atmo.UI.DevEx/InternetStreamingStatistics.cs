// ================================================================================
//
// Atmo 2
// Copyright (C) 2011  BARANI DESIGN
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// 
// Contact: Jan Barani mailto:jan@baranidesign.com
//
// ================================================================================

using System;
using System.Linq;
using System.Windows.Forms;
using Atmo.Data;
using Atmo.Units;
using DevExpress.XtraEditors;
using System.Collections.Generic;
using System.IO;

namespace Atmo.UI.DevEx
{

    public class InternetStreamingStatistics
    {
		public InternetStreamingServer[] m_StreamingServers;
        public InternetStreamingStatistics()
        {
            m_StreamingServers = new InternetStreamingServer[3];

            m_StreamingServers[0] = new InternetStreamingServer();
            m_StreamingServers[1] = new InternetStreamingServer();
            m_StreamingServers[2] = new InternetStreamingServer();
        }
    }

	
    public class InternetStreamingServer
    {
        static object _mylock = new object();

        private string m_Name = "NA";
        private long m_SendetPackets = 0;
        private long m_ReadedPackets = 0;

        public InternetStreamingServer()
        {
            m_Name = "NA";
            m_SendetPackets = 0;
            m_ReadedPackets = 0;
        }

        public void SetName(string name)
        {
            lock (_mylock)
            {
                m_Name = name;
            }
        }
        public void SetSendetPacekets(long value)
        {
            lock (_mylock)
            {
                m_SendetPackets = value;
            }
        }
        public void SetReadedPacekets(long value)
        {
            lock (_mylock)
            {
                m_ReadedPackets = value;
            }
        }

        public void IncrementSendetPacekets()
        {
            lock (_mylock)
            {
                m_SendetPackets++;
            }
        }
        public void IncrementReadedPacekets()
        {
            lock (_mylock)
            {
                m_ReadedPackets++;
            }
        }



        public string GetName()
        {
            return m_Name;
        }
        public long GetSendetPacekets()
        {
            return m_SendetPackets;
        }
        public long GetReadedPacekets()
        {
            return m_ReadedPackets;
        }

    }
}
