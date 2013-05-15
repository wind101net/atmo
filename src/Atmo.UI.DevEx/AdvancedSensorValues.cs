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
    
    class AdvancedSensorValues
    {
        static object _mylock = new object();

        private double[] m_measuredVALUES_WU;

        public AdvancedSensorValues()
        {
            //m_measuredVALUES_WU = new double[48]; // 48 = 2.5s pocas 120s = 2min
            m_measuredVALUES_WU = new double[24]; // 24 = 5s pocas 120s = 2min
        }

        public void AddValue_WU_WindSpeed(double nWindSpeed)
        {
            double tmp;
            int i;

            try
            {

                lock (_mylock)
                {
                    tmp = 0;
                    for (i = (m_measuredVALUES_WU.Length - 1); i > 0; i--)
                    {
                        m_measuredVALUES_WU[i] = m_measuredVALUES_WU[i - 1];
                    }
                    m_measuredVALUES_WU[0] = nWindSpeed;


                    for (i = 0; i < m_measuredVALUES_WU.Length; i++)
                    {
                        if (m_measuredVALUES_WU[i] > tmp)
                        {
                            tmp = m_measuredVALUES_WU[i];
                        }

                        //                File.AppendAllText("weatherlog_out.txt", "for2(" + i.ToString() + "): " + m_measuredVALUES_WU[i].ToString() + "" + Environment.NewLine);


                    }

                    m_windgust_WU = tmp;
                }
            }
            catch (Exception ex)
            {

            }



            //rp debug only: 
            //File.AppendAllText("weatherlog_out.txt", "m_windgust_WU: " + m_windgust_WU.ToString() + Environment.NewLine);




        }

        public   double m_windgust_WU = 0;

    }
}
