using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Data;
using System.IO;
using aspNetMime;
using aspNetPOP3;
using AdvancedIntellect;
//using System.Web.Mail;
using System.Net.Mail;


namespace OnLine.Pages
{
    public partial class DispComm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String[] contextId = Request.QueryString.GetValues("context");
                String[] sourceEnt = Request.QueryString.GetValues("source");
                String[] destEnt = Request.QueryString.GetValues("destination");
                String cacheKey = contextId[0] + "-" + sourceEnt[0] + "-" + destEnt[0];
                Cache.Remove(cacheKey + "-" + "msg");
                Cache.Remove(cacheKey + "-" + "attach");

                fillGrid();
                ReceiveMails();

            }
        }

        protected void fillGrid()
        {
            String[] contextId = Request.QueryString.GetValues("context");
            String[] sourceEnt = Request.QueryString.GetValues("source");
            String[] destEnt = Request.QueryString.GetValues("destination");

            SortedDictionary<DateTime, BackEndObjects.Communications> allComm = BackEndObjects.Communications.
                getAllCommunicationsforContextIdBetweenSourceAndDestDB(contextId[0], sourceEnt[0], destEnt[0]);

            DataTable dt = new DataTable();

            dt.Columns.Add("HiddenComm");
            dt.Columns.Add("RespDateTime");
            dt.Columns.Add("FromUsr");
            dt.Columns.Add("FromComp");
            dt.Columns.Add("Comments");
            int counter = 0;

            foreach (KeyValuePair<DateTime, BackEndObjects.Communications> kvp in allComm)
            {
                BackEndObjects.Communications commObj = kvp.Value;
                dt.Rows.Add();
                dt.Rows[counter]["HiddenComm"] = commObj.getCommId();
                dt.Rows[counter]["RespDateTime"] = commObj.getDateCreated();
                dt.Rows[counter]["FromUsr"] = commObj.getFromUserId();
                dt.Rows[counter]["FromComp"] = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(commObj.getFromEntityId()).getEntityName();
                dt.Rows[counter]["Comments"] = commObj.getText();

                counter++;
            }

            if (dt.Rows.Count > 0)
            {
                GridView_RFQ_Resp_Quotes_Comm.DataSource = dt;
                GridView_RFQ_Resp_Quotes_Comm.DataBind();
                GridView_RFQ_Resp_Quotes_Comm.Visible = true;
                GridView_RFQ_Resp_Quotes_Comm.Columns[0].Visible = false;
            }
        }

        protected void GridView_RFQ_Resp_Quotes_Comm_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GridView_RFQ_Resp_Quotes_Comm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button_Submit_Comment_Click(object sender, EventArgs e)
        {
            String[] contextId = Request.QueryString.GetValues("context");
            String[] sourceEnt = Request.QueryString.GetValues("source");
            String[] destEnt = Request.QueryString.GetValues("destination");

            //SortedDictionary<DateTime, BackEndObjects.Communications> allComm = BackEndObjects.Communications.
                //getAllCommunicationsforContextIdBetweenSourceAndDestDB(contextId[0], sourceEnt[0], destEnt[0]);

            BackEndObjects.Communications commObj = new BackEndObjects.Communications();
            commObj.setCommId(new Id().getNewId(Id.ID_TYPE_COMM_STRING));
            commObj.setContextId(contextId[0]);
            commObj.setDateCreated(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            commObj.setFromEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            commObj.setFromUserId(User.Identity.Name);
            commObj.setText(TextBox_Comments.Text);
            commObj.setToEntityId(sourceEnt[0]);
            commObj.setToUserId("");
            commObj.setContextType(Communications.COMMUNICATIONS_CONTEXT_TYPE_COMM);
            try
            {
                BackEndObjects.Communications.insertCommunicationDB(commObj);
                Label_Insert_Stat.Visible = true;
                Label_Insert_Stat.Text = "Comments Sent Successfully";
                Label_Insert_Stat.ForeColor = System.Drawing.Color.Green;

                fillGrid();
            }
            catch (Exception ex)
            {
                Label_Insert_Stat.Visible = true;
                Label_Insert_Stat.Text = "Comments Could not be sent";
                Label_Insert_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }

        public POP3 getPop3Connection()
        {
            String Pop3URL = "pop.gmail.com";
            String id = "shibassmb@gmail.com";
            String pass = "Chotku@123";
            POP3.LoadLicenseFile("E:\\Softwares\\EmailAsp.net\\aspNetPOP3\\aspNetPOP3.xml.lic");
            POP3 pop3Client = new POP3(Pop3URL, id, pass, 995);
            AdvancedIntellect.Ssl.SslSocket ssl = new AdvancedIntellect.Ssl.SslSocket();
            pop3Client.LoadSslSocket(ssl);

            pop3Client.Connect();

            return pop3Client;
        }

        protected void ReceiveMails()
        {
            POP3 pop3Client = getPop3Connection();
            int msgCount = pop3Client.MessageCount();


            String[] contextId = Request.QueryString.GetValues("context");
            String[] sourceEnt = Request.QueryString.GetValues("source");
            String[] destEnt = Request.QueryString.GetValues("destination");

            String cacheKey = contextId[0] + "-" + sourceEnt[0] + "-" + destEnt[0];
            Dictionary<String, String> cacheMsgDict = new Dictionary<String, String>();
            Dictionary<String, String> cacheAttachDict = new Dictionary<String, String>();

            DataTable dt = new DataTable();
            DataTable dtSession = (DataTable)Session[cacheKey];
            Dictionary<String, String> refreshCheck = new Dictionary<String, String >();

            dt.Columns.Add("Hidden");
            dt.Columns.Add("Sender");
            dt.Columns.Add("RecvDate");
            dt.Columns.Add("Subject");


            int msgCounter = 0;

            for (int i = msgCount; i > (msgCount - GridView_Email.PageSize); i--)
            {
//                if (((Dictionary<String, String>)Cache.Get(cacheKey + "-" + "msg") ==null||((Dictionary<String, String>)Cache.Get(cacheKey + "-" + "msg")).Count==0)
     //               || !((Dictionary<String, String>)Cache.Get(cacheKey + "-" + "msg")).ContainsKey((i - 1).ToString()))
          //      {
                                
                    MimeMessage msg = pop3Client.GetMessage(i - 1);
                    if (i > (msgCount - 1))
                    {
                        Boolean messageInCache=false;

                        if( ((Dictionary<String, String>)Cache.Get(cacheKey + "-" + "msg"))!=null && ((Dictionary<String, String>)Cache.Get(cacheKey + "-" + "msg")).ContainsKey((i - 1).ToString()) )
                        messageInCache=true;

                        if (!messageInCache)
                        {
                            MimePartCollection allMimeParts = msg.MimeParts;
                            String messageText = "";
                            String attachmentName = "";

                            if (msg.TextMimePart != null)
                                messageText += msg.TextMimePart.ToString();
                            else
                                messageText = "";

                            if (i == msgCount)
                                TextBox_MsgBody.Text = msg.TextMimePart.ToString();
                            //MessagBodyHTML.InnerHtml = msg.TextMimePart.ToString();

                            for (int j = 0; j < allMimeParts.Count; j++)
                            {
                                if (allMimeParts[j].IsAttachment())
                                {
                                    if (i == msgCount)
                                        LabelAttachList.Text += LabelAttachList.Text.Equals("") ? allMimeParts[j].AttachmentName() : "," + allMimeParts[j].AttachmentName();
                                    attachmentName += attachmentName.Equals("") ? allMimeParts[j].AttachmentName() : "," + allMimeParts[j].AttachmentName();
                                }
                            }

                            cacheMsgDict.Add((i - 1).ToString(), messageText);
                            cacheAttachDict.Add((i - 1).ToString(), attachmentName);
                        }
                    }
                    if (msg != null)
                    {
                        dt.Rows.Add();
                        dt.Rows[msgCounter]["Hidden"] = i.ToString();
                        dt.Rows[msgCounter]["Sender"] = msg.From.EmailAddress;
                        dt.Rows[msgCounter]["RecvDate"] = msg.ReceivedDate().Date;
                        dt.Rows[msgCounter]["Subject"] = msg.Subject.ToString();

                        refreshCheck.Add(i.ToString(), i.ToString());

                        msgCounter++;
                    }
            }


            Cache.Insert(cacheKey + "-" + "msg", cacheMsgDict);
            Cache.Insert(cacheKey + "-" + "attach", cacheAttachDict);
            Cache.Insert(cacheKey, dt);

            Session[SessionFactory.POP3_CLIENT_OBJECT] = pop3Client;
            
            //Now append the already existing datatable from session (if any)
            int rowCountIndex=dt.Rows.Count;
            int counter = 0;
            if (dtSession != null)
                for (int j = 0; j < dtSession.Rows.Count; j++)
                {
                    if (!refreshCheck.ContainsKey(dtSession.Rows[j]["Hidden"].ToString()))
                    {
                        dt.Rows.Add();
                        dt.Rows[rowCountIndex + counter]["Hidden"] = dtSession.Rows[counter]["Hidden"];
                        dt.Rows[rowCountIndex + counter]["Sender"] = dtSession.Rows[counter]["Sender"];
                        dt.Rows[rowCountIndex + counter]["RecvDate"] = dtSession.Rows[counter]["RecvDate"];
                        dt.Rows[rowCountIndex + counter]["Subject"] = dtSession.Rows[counter]["Subject"];

                        counter++;
                    }
                }
        
            GridView_Email.Visible = true;
            GridView_Email.DataSource = dt;
            GridView_Email.DataBind();
            GridView_Email.Columns[0].Visible = false;
            GridView_Email.SelectedIndex = 0;
            LabelSelectedGridItem.Text = ((Label)GridView_Email.Rows[0].Cells[0].FindControl("Label_Hidden")).Text;
            Session[cacheKey] = dt;
        }

  
        protected void GridView_Lead_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void LinkButton_Subject_Command(object sender, CommandEventArgs e)
        {

        }

        protected void LinkButton_Subject_Command1(object sender, CommandEventArgs e)
        {
            try
            {
                GridView_Email.SelectedIndex = int.Parse(e.CommandArgument.ToString());
                LabelSelectedGridItem.Text = ((Label)GridView_Email.Rows[int.Parse(e.CommandArgument.ToString())].Cells[0].FindControl("Label_Hidden")).Text;

                POP3 pop3Client = (POP3)Session[SessionFactory.POP3_CLIENT_OBJECT];
                String[] contextId = Request.QueryString.GetValues("context");
                String[] sourceEnt = Request.QueryString.GetValues("source");
                String[] destEnt = Request.QueryString.GetValues("destination");

                String cacheKey = contextId[0] + "-" + sourceEnt[0] + "-" + destEnt[0];

                String index = ((Label)GridView_Email.Rows[int.Parse(e.CommandArgument.ToString())].Cells[0].FindControl("Label_Hidden")).Text;

                if (((Dictionary<String, String>)Cache.Get(cacheKey + "-" + "msg")).ContainsKey((float.Parse(index) - 1).ToString()))
                {
                    TextBox_MsgBody.Text = ((Dictionary<String, String>)Cache.Get(cacheKey + "-" + "msg"))[(float.Parse(index) - 1).ToString()];
                    LabelAttachList.Text = ((Dictionary<String, String>)Cache.Get(cacheKey + "-" + "attach"))[(float.Parse(index) - 1).ToString()];
                }
                else
                {
                    //Message not in cache..pull from server
                    if (!pop3Client.IsConnected)
                    {
                        pop3Client.Connect();
                        Session[SessionFactory.POP3_CLIENT_OBJECT] = pop3Client;
                    }
                    MimeMessage msg = pop3Client.GetMessage(int.Parse(index) - 1);

                    Dictionary<String, String> cacheMsgDict = (Dictionary<String, String>)Cache.Get(cacheKey + "-" + "msg");
                    Dictionary<String, String> cacheAttachDict = (Dictionary<String, String>)Cache.Get(cacheKey + "-" + "attach");

                    String messageText = msg.TextMimePart.ToString();
                    String attachmentName = "";

                    MimePartCollection mimePartCollc = msg.MimeParts;
                    for (int j = 0; j < mimePartCollc.Count; j++)
                        if (mimePartCollc[j].IsAttachment())
                            attachmentName = attachmentName.Equals("") ? mimePartCollc[j].AttachmentName() :
                                attachmentName + "," + mimePartCollc[j].AttachmentName();

                    cacheMsgDict.Add((int.Parse(index)-1).ToString(), messageText);
                    cacheAttachDict.Add((int.Parse(index)-1).ToString(), attachmentName);

                    Cache.Insert(cacheKey + "-" + "msg", cacheMsgDict);
                    Cache.Insert(cacheKey + "-" + "attach", cacheAttachDict);

                    TextBox_MsgBody.ForeColor = System.Drawing.Color.Black;
                    
                    TextBox_MsgBody.Text = messageText;
                    LabelAttachList.Text = attachmentName;

                }
            }
            catch (Exception ex)
            {
                TextBox_MsgBody.Text = "Error Retrieving Message Content";
                TextBox_MsgBody.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void ButtonSend_Click(object sender, EventArgs e)
        {
            ButtonDone.Enabled = false;
            LabelSending.Visible = true;
            sendMail("shibassnb@gmail.com", TextBoxTo.Text, TextBoxCc.Text, "", TextBoxSubject.Text, TextBoxCompEmailBody.Text);
            ButtonDone.Enabled = true;
            LabelSending.Visible = false;
        }

        public string sendMail(string from, string to, string cc, string bcc, string subject, string body)
        {
            // Mail initialization
            System.Net.Mail.MailMessage mailMsg= new MailMessage();
            
            mailMsg.From = new MailAddress(from);
            mailMsg.To.Add(to);
            if(!cc.Equals(""))
            mailMsg.CC.Add(cc);
            //mailMsg.Bcc = bcc;
            mailMsg.Subject = subject;
            //mailMsg.BodyFormat = MailFormat.Text;
            mailMsg.Body = body;
            mailMsg.Priority = MailPriority.High;

            if (FileUpload1.HasFile)
            {
                // File Upload path
                //String FileName = FileUpload1.PostedFile.FileName;
                //Getting Attachment file
                //MailAttachment mailAttachment = new MailAttachment(FileName, MailEncoding.Base64);
                //Attaching uploaded file
                //mailMsg.Attachments.Add(mailAttachment);
                mailMsg.Attachments.Add(new Attachment(FileUpload1.FileContent, System.IO.Path.GetFileName(FileUpload1.FileName)));
            }
            if (FileUpload2.HasFile)
            {
                mailMsg.Attachments.Add(new Attachment(FileUpload2.FileContent, System.IO.Path.GetFileName(FileUpload2.FileName)));
            }
            if (FileUpload3.HasFile)
            {
                mailMsg.Attachments.Add(new Attachment(FileUpload3.FileContent, System.IO.Path.GetFileName(FileUpload3.FileName)));
            }
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("shibassmb@gmail.com", "Chotku@123");
            client.Port = 25;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            client.Send(mailMsg);
            // Smtp configuration
            //SmtpMail.SmtpServer = "smtp.gmail.com";//smtp is :smtp.gmail.com
            // - smtp.gmail.com use smtp authentication
            //mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            //mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "shibassmb@gmail.com");
            //mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "Chotku@123");
            // - smtp.gmail.com use port 465 or 587
            //mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", "465");//port is: 465
            // - smtp.gmail.com use STARTTLS (some call this SSL)
            //mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
            // try to send Mail
            try
            {
                //SmtpMail.Send(mailMsg);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        protected void ButtonCompose_Click(object sender, EventArgs e)
        {
            PanelCompEmail.Visible = true;
        }

        protected void GridView_Email_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void TextBoxCompEmailBody_TextChanged(object sender, EventArgs e)
        {
            PanelCompEmail.Visible = false;
        }

        protected void ButtonMoreAttach_Click(object sender, EventArgs e)
        {
            
        }

        protected void ButtonAddMore_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
                if (!FileUpload2.HasFile)
                    FileUpload2.Visible = true;
                else if (!FileUpload3.HasFile)
                    FileUpload3.Visible = true;
        }

        protected void ButtonDone_Click(object sender, EventArgs e)
        {
            PanelCompEmail.Visible = false;
        }

        protected void ButtonNextPage_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonDownload_Click(object sender, EventArgs e)
        {
            String[] contextId = Request.QueryString.GetValues("context");
            String[] sourceEnt = Request.QueryString.GetValues("source");
            String[] destEnt = Request.QueryString.GetValues("destination");

            
            DataTable dt = new DataTable();
            dt.Columns.Add("attachment");

            POP3 pop3Client = (POP3)Session[SessionFactory.POP3_CLIENT_OBJECT];
            pop3Client = getPop3Connection();
            //if (!pop3Client.IsConnected)
                //pop3Client.Connect();
            
            MimeMessage msg = pop3Client.GetMessage(int.Parse(LabelSelectedGridItem.Text.ToString())-1);
            MimePartCollection mimeCollc = msg.MimeParts;
            String attachmentName = "";
            
            String downloadReadyMsg=contextId[0]+"-"+sourceEnt[0]+"-"+destEnt[0]+"-"+SessionFactory.ALL_COMM_DOWNLOADABLE_MSG;
            Session[downloadReadyMsg] = msg;

            /*for (int i = 0; i < mimeCollc.Count; i++)
            {
                if (mimeCollc[i].IsAttachment())
                    attachmentName += attachmentName.Equals("") ? mimeCollc[i].AttachmentName() : "," + mimeCollc[i].AttachmentName();
            }*/

            attachmentName = LabelAttachList.Text;
            String[] attachmentNameArray = attachmentName.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            if (attachmentNameArray.Length==1)
                DownloadAttachment(msg, attachmentName);

            else
            {                

                for (int i = 0; i < attachmentNameArray.Length; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["attachment"] = attachmentNameArray[i];
                }

                PanelDownload.Visible = true;
                GridView_Attachment.Visible = true;
                GridView_Attachment.DataSource = dt;
                GridView_Attachment.DataBind();

            }
        }

        protected void DownloadAttachment(MimeMessage msg,String attachmentName)
        {
                        Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Cookies.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + attachmentName);
            Response.ContentType = ((String[])attachmentName.Split(new String[] { "." }, StringSplitOptions.None))[1];

            int len = (int)msg.GetAttachment(attachmentName).DataStream().Length;
            byte[] bufferToSend = new byte[len];

            msg.GetAttachment(attachmentName).DataStream().
                Read(bufferToSend, 0, len);

            Response.BinaryWrite(bufferToSend);
        }

        protected void GridView_Attachment_SelectedIndexChanged(object sender, EventArgs e)
        {
                        String[] contextId = Request.QueryString.GetValues("context");
            String[] sourceEnt = Request.QueryString.GetValues("source");
            String[] destEnt = Request.QueryString.GetValues("destination");

            String attachmentName=((Label)GridView_Attachment.Rows[GridView_Attachment.SelectedIndex].Cells[0].FindControl("Label_Attachment")).Text;
            DownloadAttachment((MimeMessage)Session[contextId[0] + "-" + sourceEnt[0] + "-" + destEnt[0] + "-" + SessionFactory.ALL_COMM_DOWNLOADABLE_MSG], attachmentName);
        }

        protected void ButtonHideAttachment_Click(object sender, EventArgs e)
        {
            PanelDownload.Visible = false;
            GridView_Attachment.Visible = false;
        }

        protected void ButtonRefreshEmail_Click(object sender, EventArgs e)
        {
            ReceiveMails();
        }
    }
}