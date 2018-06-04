using System.Configuration;
using System.Windows.Forms;
using System;
using Aliyun.Api.LOG.Request;
using Aliyun.Api.LOG;
using Aliyun.Api.LOG.Response;
using Aliyun.Api.LOG.Data;
using System.Text;

namespace TestAliyunLog
{
    public partial class Form1 : Form
    {
        static readonly string accesskey = ConfigurationManager.AppSettings["aliyunaccesskey"].ToString().Trim();
        static readonly string accessecret = ConfigurationManager.AppSettings["aliyunaccessecret"].ToString().Trim();
        static readonly string endpoint = ConfigurationManager.AppSettings["aliyunendpoint"].ToString().Trim();
        static readonly string bucketname = ConfigurationManager.AppSettings["aliyunbucketname"].ToString().Trim();
        static readonly string projectName = ConfigurationManager.AppSettings["aliyunprojectname"].ToString().Trim();
        static readonly string logstore = ConfigurationManager.AppSettings["aliyunlogstore"].ToString().Trim();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            GetAliyunLog();
        }

        private void GetAliyunLog()
        {
            LogClient _client = new LogClient(endpoint, accesskey, accessecret);

            StringBuilder sb = new StringBuilder();
            DateTime unixTimestampZeroPoint = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            DateTime fromStamp = DateTime.UtcNow.AddDays(-1);
            DateTime toStamp = DateTime.UtcNow;
            sb.Append("beginTime:" + fromStamp + "--" + "endTime" + toStamp+"\n\n");

            uint from = (uint)((fromStamp - unixTimestampZeroPoint).TotalSeconds);
            uint to = (uint)((toStamp - unixTimestampZeroPoint).TotalSeconds);
            try
            {
                GetLogsRequest req = new GetLogsRequest(projectName, logstore, from, to, "", "ActivityLogTypeName:网站商品PV |  select  ActivityLogTypeName, Comment, count(1) as count  group by Comment,ActivityLogTypeName order by count  desc limit 100", int.MaxValue, 0, true);
                GetLogsResponse res = _client.GetLogs(req);


                if (res != null && res.IsCompleted())
                {
                    foreach (QueriedLog log in res.Logs)
                    {
                        foreach (var item in log.Contents)
                        {
                            sb.Append(item.Key + ";" + item.Value + "\n");
                        }
                    }
                }
                MessageBox.Show(sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
    }
}
