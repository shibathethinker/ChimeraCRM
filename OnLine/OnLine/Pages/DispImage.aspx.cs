using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using BackEndObjects;
using ActionLibrary;


namespace OnLine.Pages
{
    public partial class DispImage1 : System.Web.UI.Page
    {
        protected static String SERVER_PATH="serverPath";
        protected static String CONTENT_TYPE="contentType";
        public  static Dictionary<String, String> MIMEList=null;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            MIMEList =(Dictionary<String,String>)Cache["AllMimeTypes"];

            if(MIMEList==null || MIMEList.Count==0)
            {
           MIMEList= new Dictionary<string, string>();
                loadMimeTypes();
            }

                Dictionary<String,String> contentAndPath=getContentTypeandPath();

                string filePath = contentAndPath[SERVER_PATH];

                if (filePath != null && !filePath.Equals(""))
                {
                    String[] fileNameParts = filePath.Split(new String[]{"\\"}, StringSplitOptions.RemoveEmptyEntries);

                    Response.ContentType = contentAndPath[CONTENT_TYPE];
                    //Response.AddHeader("Content-Type", contentAndPath[CONTENT_TYPE]);
                    Response.AddHeader("Content-Disposition", "attachment; filename="+fileNameParts[fileNameParts.Length-1]);
                    Response.WriteFile(filePath);
                    Response.End();
                }
                else
                    Label1.Visible = true;
            }
        }

        protected void loadMimeTypes()
        {
            MIMEList.Add("ai", "application/postscript");
            MIMEList.Add("aif", "audio/x-aiff");
            MIMEList.Add("aifc", "audio/x-aiff");
            MIMEList.Add("aiff", "audio/x-aiff");
            MIMEList.Add("asc", "text/plain");
            MIMEList.Add("asf", "video/x.ms.asf");
            MIMEList.Add("asx", "video/x.ms.asx");
            MIMEList.Add("au", "audio/basic");
            MIMEList.Add("avi", "video/x-msvideo");
            MIMEList.Add("bcpio", "application/x-bcpio");
            MIMEList.Add("bin", "application/octet-stream");
            MIMEList.Add("cab", "application/x-cabinet");
            MIMEList.Add("cdf", "application/x-netcdf");
            MIMEList.Add("class", "application/java-vm");
            MIMEList.Add("cpio", "application/x-cpio");
            MIMEList.Add("cpt", "application/mac-compactpro");
            MIMEList.Add("crt", "application/x-x509-ca-cert");
            MIMEList.Add("csh", "application/x-csh");
            MIMEList.Add("css", "text/css");
            MIMEList.Add("csv", "text/comma-separated-values");
            MIMEList.Add("dcr", "application/x-director");
            MIMEList.Add("dir", "application/x-director");
            MIMEList.Add("dll", "application/x-msdownload");
            MIMEList.Add("dms", "application/octet-stream");
            MIMEList.Add("doc", "application/msword");
            MIMEList.Add("dtd", "application/xml-dtd");
            MIMEList.Add("dvi", "application/x-dvi");
            MIMEList.Add("dxr", "application/x-director");
            MIMEList.Add("eps", "application/postscript");
            MIMEList.Add("etx", "text/x-setext");
            MIMEList.Add("exe", "application/octet-stream");
            MIMEList.Add("ez", "application/andrew-inset");
            MIMEList.Add("gif", "image/gif");
            MIMEList.Add("gtar", "application/x-gtar");
            MIMEList.Add("gz", "application/gzip");
            MIMEList.Add("gzip", "application/gzip");
            MIMEList.Add("hdf", "application/x-hdf");
            MIMEList.Add("htc", "text/x-component");
            MIMEList.Add("hqx", "application/mac-binhex40");
            MIMEList.Add("html", "text/html");
            MIMEList.Add("htm", "text/html");
            MIMEList.Add("ice", "x-conference/x-cooltalk");
            MIMEList.Add("ief", "image/ief");
            MIMEList.Add("iges", "model/iges");
            MIMEList.Add("igs", "model/iges");
            MIMEList.Add("jar", "application/java-archive");
            MIMEList.Add("java", "text/plain");
            MIMEList.Add("jnlp", "application/x-java-jnlp-file");
            MIMEList.Add("jpeg", "image/jpeg");
            MIMEList.Add("jpe", "image/jpeg");
            MIMEList.Add("jpg", "image/jpeg");
            MIMEList.Add("js", "application/x-javascript");
            MIMEList.Add("jsp", "text/plain");
            MIMEList.Add("kar", "audio/midi");
            MIMEList.Add("latex", "application/x-latex");
            MIMEList.Add("lha", "application/octet-stream");
            MIMEList.Add("lzh", "application/octet-stream");
            MIMEList.Add("man", "application/x-troff-man");
            MIMEList.Add("mathml", "application/mathml+xml");
            MIMEList.Add("me", "application/x-troff-me");
            MIMEList.Add("mesh", "model/mesh");
            MIMEList.Add("mid", "audio/midi");
            MIMEList.Add("midi", "audio/midi");
            MIMEList.Add("mif", "application/vnd.mif");
            MIMEList.Add("mol", "chemical/x-mdl-molfile");
            MIMEList.Add("movie", "video/x-sgi-movie");
            MIMEList.Add("mov", "video/quicktime");
            MIMEList.Add("mp2", "audio/mpeg");
            MIMEList.Add("mp3", "audio/mpeg");
            MIMEList.Add("mpeg", "video/mpeg");
            MIMEList.Add("mpe", "video/mpeg");
            MIMEList.Add("mpga", "audio/mpeg");
            MIMEList.Add("mpg", "video/mpeg");
            MIMEList.Add("ms", "application/x-troff-ms");
            MIMEList.Add("msg", "application/vnd.ms-outlook");
            MIMEList.Add("msh", "model/mesh");
            MIMEList.Add("msi", "application/octet-stream");
            MIMEList.Add("nc", "application/x-netcdf");
            MIMEList.Add("oda", "application/oda");
            MIMEList.Add("ogg", "application/ogg");
            MIMEList.Add("pbm", "image/x-portable-bitmap");
            MIMEList.Add("pdb", "chemical/x-pdb");
            MIMEList.Add("pdf", "application/pdf");
            MIMEList.Add("pgm", "image/x-portable-graymap");
            MIMEList.Add("pgn", "application/x-chess-pgn");
            MIMEList.Add("png", "image/png");
            MIMEList.Add("pnm", "image/x-portable-anymap");
            MIMEList.Add("ppm", "image/x-portable-pixmap");
            MIMEList.Add("ppt", "application/vnd.ms-powerpoint");
            MIMEList.Add("ps", "application/postscript");
            MIMEList.Add("qt", "video/quicktime");
            MIMEList.Add("ra", "audio/x-pn-realaudio");            
            MIMEList.Add("ram", "audio/x-pn-realaudio");
            MIMEList.Add("ras", "image/x-cmu-raster");
            MIMEList.Add("rdf", "application/rdf+xml");
            MIMEList.Add("rgb", "image/x-rgb");
            MIMEList.Add("rm", "audio/x-pn-realaudio");
            MIMEList.Add("roff", "application/x-troff");
            MIMEList.Add("rpm", "application/x-rpm");            
            MIMEList.Add("rtf", "application/rtf");
            MIMEList.Add("rtx", "text/richtext");
            MIMEList.Add("ser", "application/java-serialized-object");
            MIMEList.Add("sgml", "text/sgml");
            MIMEList.Add("sgm", "text/sgml");
            MIMEList.Add("sh", "application/x-sh");
            MIMEList.Add("shar", "application/x-shar");
            MIMEList.Add("silo", "model/mesh");
            MIMEList.Add("sit", "application/x-stuffit");
            MIMEList.Add("skd", "application/x-koan");
            MIMEList.Add("skm", "application/x-koan");
            MIMEList.Add("skp", "application/x-koan");
            MIMEList.Add("skt", "application/x-koan");
            MIMEList.Add("smi", "application/smil");
            MIMEList.Add("smil", "application/smil");
            MIMEList.Add("snd", "audio/basic");
            MIMEList.Add("spl", "application/x-futuresplash");
            MIMEList.Add("src", "application/x-wais-source");
            MIMEList.Add("sv4cpio", "application/x-sv4cpio");
            MIMEList.Add("sv4crc", "application/x-sv4crc");
            MIMEList.Add("svg", "image/svg+xml");
            MIMEList.Add("swf", "application/x-shockwave-flash");
            MIMEList.Add("t", "application/x-troff");
            MIMEList.Add("tar", "application/x-tar");
            MIMEList.Add("tar.gz", "application/x-gtar");
            MIMEList.Add("tcl", "application/x-tcl");
            MIMEList.Add("tex", "application/x-tex");
            MIMEList.Add("texi", "application/x-texinfo");
            MIMEList.Add("texinfo", "application/x-texinfo");
            MIMEList.Add("tgz", "application/x-gtar");
            MIMEList.Add("tiff", "image/tiff");
            MIMEList.Add("tif", "image/tiff");
            MIMEList.Add("tr", "application/x-troff");
            MIMEList.Add("tsv", "text/tab-separated-values");
            MIMEList.Add("txt", "text/plain");
            MIMEList.Add("ustar", "application/x-ustar");
            MIMEList.Add("vcd", "application/x-cdlink");
            MIMEList.Add("vrml", "model/vrml");
            MIMEList.Add("vxml", "application/voicexml+xml");
            MIMEList.Add("wav", "audio/x-wav");
            MIMEList.Add("wbmp", "image/vnd.wap.wbmp");
            MIMEList.Add("wmlc", "application/vnd.wap.wmlc");
            MIMEList.Add("wmlsc", "application/vnd.wap.wmlscriptc");
            MIMEList.Add("wmls", "text/vnd.wap.wmlscript");
            MIMEList.Add("wml", "text/vnd.wap.wml");
            MIMEList.Add("wrl", "model/vrml");
            MIMEList.Add("wtls-ca-certificate", "application/vnd.wap.wtls-ca-certificate");
            MIMEList.Add("xbm", "image/x-xbitmap");
            MIMEList.Add("xht", "application/xhtml+xml");
            MIMEList.Add("xhtml", "application/xhtml+xml");
            MIMEList.Add("xls", "application/vnd.ms-excel");
            MIMEList.Add("xml", "application/xml");
            MIMEList.Add("xpm", "image/x-xpixmap");            
            MIMEList.Add("xsl", "application/xml");
            MIMEList.Add("xslt", "application/xslt+xml");
            MIMEList.Add("xul", "application/vnd.mozilla.xul+xml");
            MIMEList.Add("xwd", "image/x-xwindowdump");
            MIMEList.Add("xyz", "chemical/x-xyz");
            MIMEList.Add("z", "application/compress");
            MIMEList.Add("zip", "application/zip");
            MIMEList.Add("rar", "application/x-rar-compressed");
            MIMEList.Add("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            MIMEList.Add("xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template");
            MIMEList.Add("potx", "application/vnd.openxmlformats-officedocument.presentationml.template");
            MIMEList.Add("ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow");
            MIMEList.Add("pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            MIMEList.Add("sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide");
            MIMEList.Add("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            MIMEList.Add("dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template");
            MIMEList.Add("xlam", "application/vnd.ms-excel.addin.macroEnabled.12");
            MIMEList.Add("xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12");
        }

        protected Dictionary<String, String> getContentTypeandPath()
        {
            ActionLibrary.ImageContextFactory icObj = (ActionLibrary.ImageContextFactory)Session[SessionFactory.DISP_IMAGE_CONTEXT_FACTORY_OBJ];
            DataTable dt = new DataTable();
            dt.Columns.Add("img");
            String serverPath = "";
            String contentType = "";
            Dictionary<String, String> pathAndContent = new Dictionary<string, string>();

            switch (icObj.getParentContextName())
            {
                case ActionLibrary.ImageContextFactory.PARENT_CONTEXT_NOTES:
                    if(icObj.getDestinationContextName().Equals
                        (ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_DOC_FOR_PARENT_NOTE))
                    {
                        serverPath=BackEndObjects.Communications.getCommunicationbyIdDB(icObj.getParentContextValue()).getDocPath();
                        contentType=getMimeType(serverPath);
                    }

                    break;

                case ActionLibrary.ImageContextFactory.PARENT_CONTEXT_REQUIREMENT:
                    int counter = 0;
                    if (icObj.getDestinationContextName().Equals
                        (ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_FEAT_FOR_PARENT_REQUIREMENT))
                    {
                        String prodCatId = icObj.getChildContextObjects()[ActionLibrary.ImageContextFactory.CHILD_CONTEXT_PRODCAT_ID],
featId = icObj.getChildContextObjects()[ActionLibrary.ImageContextFactory.CHILD_CONTEXT_FEAT_ID];

                        ArrayList reqSpecList = BackEndObjects.Requirement_Spec.getRequirementSpecsforReqbyIdDB(icObj.getParentContextValue());


                        for (int i = 0; i < reqSpecList.Count; i++)
                        {
                            BackEndObjects.Requirement_Spec reqrObj = (Requirement_Spec)reqSpecList[i];
                            if (reqrObj.getProdCatId().Equals(prodCatId) && reqrObj.getFeatId().Equals(featId))
                            {
                                serverPath = reqrObj.getImgPath();
                                //dt.Rows.Add();
                                //dt.Rows[counter]["img"] = serverPath;
                                contentType = getMimeType(serverPath);
                                counter++;
                            }
                        }
                    }
                    break;

                case ActionLibrary.ImageContextFactory.PARENT_CONTEXT_RFQ:

                    if (icObj.getDestinationContextName().Equals(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_FEAT_FOR_PARENT_RFQ))
                    {
                        String prodCatId = icObj.getChildContextObjects()[ActionLibrary.ImageContextFactory.CHILD_CONTEXT_PRODCAT_ID],
featId = icObj.getChildContextObjects()[ActionLibrary.ImageContextFactory.CHILD_CONTEXT_FEAT_ID];

                        ArrayList rfqSpecList = BackEndObjects.RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(icObj.getParentContextValue());

                        counter = 0;
                        for (int i = 0; i < rfqSpecList.Count; i++)
                        {
                            BackEndObjects.RFQProductServiceDetails rfqObj = (RFQProductServiceDetails)rfqSpecList[i];
                            if (rfqObj.getPrdCatId().Equals(prodCatId) && rfqObj.getFeatId().Equals(featId))
                            {
                                serverPath = rfqObj.getImgPath();
                                //dt.Rows.Add();
                                //dt.Rows[counter]["img"] = serverPath;
                                contentType = getMimeType(serverPath);
                                counter++;
                            }
                        }

                    }
                    if (icObj.getDestinationContextName().Equals(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_NDA_FOR_PARENT_RFQ))
                    {
                        serverPath = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(icObj.getParentContextValue()).getNDADocPath();
                        //dt.Rows.Add();
                        //dt.Rows[0]["img"] = serverPath;
                        contentType = getMimeType(serverPath);
                    }
                    break;

                case ActionLibrary.ImageContextFactory.PARENT_CONTEXT_RFQ_RESPONSE:

                    if (icObj.getDestinationContextName().Equals(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_NDA_FOR_PARENT_RFQ_RESPONSE))
                    {
                        serverPath = BackEndObjects.RFQResponse.
                            getRFQResponseforRFQIdandResponseEntityIdDB
                            (icObj.getParentContextValue(),
                            (icObj.getChildContextObjects()[ActionLibrary.ImageContextFactory.CHILD_CONTEXT_RFQ_RESPONSE_RESPONSE_ENTITY_ID]).ToString()).getNdaPath();
                        contentType = getMimeType(serverPath);
                        //dt.Rows.Add();
                        //dt.Rows[0]["img"] = serverPath;

                    }

                    break;

                case ActionLibrary.ImageContextFactory.PARENT_CONTEXT_PRODUCT:

                    if (icObj.getDestinationContextName().Equals(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_FEAT_FOR_PARENT_PRODUCT))
                    {
                        String prodName = icObj.getParentContextValue();
                        String entId = icObj.getChildContextObjects()[ActionLibrary.ImageContextFactory.CHILD_CONTEXT_PROD_ENT_ID];
                        String featId = icObj.getChildContextObjects()[ActionLibrary.ImageContextFactory.CHILD_CONTEXT_PROD_FEAT_ID];

                        ShopChildProdsSpecs specObj = (ShopChildProdsSpecs)ShopChildProdsSpecs.
                            getShopChildProdsSpecObjbyEntIdandProdNameDB(entId, prodName)[featId];

                        serverPath = specObj.getImgPath();
                        contentType = getMimeType(serverPath);
                    }
                    break;

                case ActionLibrary.ImageContextFactory.PARENET_CONTEXT_DEFECT:

                    if(icObj.getDestinationContextName().Equals(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_DOC_FOR_PARENT_DEFECT))
                    {
                        String defectId = icObj.getParentContextValue();
                        BackEndObjects.DefectDetails defObj = BackEndObjects.DefectDetails.getDefectDetailsbyidDB(defectId);
                        serverPath = defObj.getDocPath();
                        contentType = getMimeType(serverPath);
                    }
                    break;
                    
            }

            pathAndContent.Add(SERVER_PATH, serverPath);
            pathAndContent.Add(CONTENT_TYPE, contentType);

            return pathAndContent;
        }

        protected String getMimeType(String pathName)
        {
            if (pathName != null && !pathName.Equals(""))
            {
                String[] splittedString = pathName.Split('.');
                String extn = splittedString[splittedString.Length - 1];
                return MIMEList[extn];
            }
            else
                return "";
        }
    }
}