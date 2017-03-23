using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarService.Models;
using System.Xml.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace CarService.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index()
        {

            var customers = db.Customers.Include(c => c.frequency);
            return View(customers.ToList());
        }

        public ActionResult Search123()
        {
            var customers = db.Customers.Include(c => c.frequency);
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public static int newid;
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            newid = id.GetValueOrDefault();
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.FrequencyID = new SelectList(db.Frequencies, "id", "description");
            return View();
        }

        //public static void getLatLng(Customer customer)
        //{
        //    string add = customer.street1 + "," + customer.street2 + "," + customer.city + "," + customer.state;
        //    var address = add;
        //    var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));

        //    var request = WebRequest.Create(requestUri);
        //    var response = request.GetResponse();
        //    var xdoc = XDocument.Load(response.GetResponseStream());

        //    var result = xdoc.Element("GeocodeResponse").Element("result");
        //    var locationElement = result.Element("geometry").Element("location");
        //    var lat = locationElement.Element("lat");
        //    var lng = locationElement.Element("lng");
        //    customer.latitude = (double)lat;
        //    customer.longitude = (double)lng;
        //}


        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,phoneNumber,CarModel,CarMake,CarCapacity,CarAverage,PerHourRate,Available,FrequencyID,street1,stree2,city,state")] Customer customer, DateTime FreeFrom, DateTime FreeTo)
        {
            if(ModelState.IsValid)
            {
                DateTime d = FreeFrom;
                TimeSpan timeOfDay = d.TimeOfDay;
                string freef = timeOfDay.Hours.ToString();
                DateTime d1 = FreeTo;
                TimeSpan timeOfDay1 = d1.TimeOfDay;
                string freet = timeOfDay1.Hours.ToString();
                string add = customer.street1 + "," + customer.street2 + "," + customer.city + "," + customer.state;
                var address = add;
                var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));

                var request = WebRequest.Create(requestUri);
                var response = request.GetResponse();
                var xdoc = XDocument.Load(response.GetResponseStream());

                var result = xdoc.Element("GeocodeResponse").Element("result");
                var locationElement = result.Element("geometry").Element("location");
                var lat = locationElement.Element("lat");
                var lng = locationElement.Element("lng");
                customer.latitude = (double)lat;
                customer.longitude = (double)lng;

                int num11, num22;
                bool num1 = int.TryParse(freef, out num11);
                bool num2 = int.TryParse(freet, out num22);
                customer.FreeFrom = num11;
                customer.FreeTo = num22;
                customer.email = User.Identity.Name;
                //getLatLng(customer);
            }
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FrequencyID = new SelectList(db.Frequencies, "id", "description", customer.FrequencyID);
            return View(customer);
        }


        public ActionResult Search()
        {
            List<Customer> cust1 = new List<Customer>();

            cust1 = db.Customers.Where(c => c.Available==true).ToList();
            return View(cust1);
        }

        [HttpPost]
        public ActionResult Search(DateTime? freefrom, DateTime? freeto)
        {
            // flag if search has been performed
            List<Customer> cust1 = new List<Customer>();
            List<Customer> cust2 = new List<Customer>();
            cust1 = db.Customers.Where(c => c.Available == true).ToList();
            Boolean searchPerformed = false;


            if (freefrom.ToString() == null && freeto.ToString() == null)
            {
                return View(cust1);
            }
            DateTime d = freefrom.GetValueOrDefault();
            TimeSpan timeOfDay = d.TimeOfDay;
            string freef = timeOfDay.Hours.ToString();

            DateTime d1 = freeto.GetValueOrDefault();
            TimeSpan timeOfDay1 = d1.TimeOfDay;
            string freet = timeOfDay1.Hours.ToString();
            //string freef = d.ToShortTimeString();

            //for (int i= freef.Length; i> 0; i -- )
            //{
            //    if(freef[i-1].Equals("P")) 
            //}
            int num11, num22;
            bool num1 = int.TryParse(freef, out num11);
            bool num2 = int.TryParse(freet, out num22);
            cust2 = db.Customers.Where(c => c.FreeFrom >= num11 && c.FreeTo <= num22 && c.Available == true).ToList();
            if (cust2.Count != 0)
            {
                searchPerformed = true;
            }
            //if (!(String.IsNullOrWhiteSpace(freeto) && String.IsNullOrWhiteSpace(freefrom)))
            //{
            //    searchPerformed = true;
            //}


            //    //var cust = db.Customers;
            //    int searchfreefrom;
            //    int searchfreeto;
            //    bool num1;
            //    bool num2;
            //    num1 = int.TryParse(freefrom, out searchfreefrom);
            //    num2 = int.TryParse(freeto, out searchfreeto);



            //foreach (var c in cust1)
            //{

            //    int freefrominteger;
            //    int freetointeger;
            //    bool test;
            //    bool test2;
            //    test = int.TryParse(c.FreeFrom, out freefrominteger);
            //    test2 = int.TryParse(c.FreeTo, out freetointeger);
            //    if (freefrominteger >= searchfreefrom && freetointeger <= searchfreeto && c.Available==true)
            //    {
            //        cust2.Add(c);
            //    }

            //}

            //ViewBag.customerlist = cust2;

            // filter first names if firstName search value provided
            //if (!string.IsNullOrWhiteSpace(firstName))
            //{
            //    contacts = contacts.Where(x => x.FirstName.Contains(firstName));
            //    searchPerformed = true;
            //}

            //// filter last names if lastName search value provided
            //if (!string.IsNullOrWhiteSpace(lastName))
            //{
            //    contacts = contacts.Where(x => x.LastName.Contains(lastName));
            //    searchPerformed = true;
            //}

            //// filter phone numbers if phone search value provided
            //if (!string.IsNullOrWhiteSpace(phone))
            //{
            //    contacts = contacts.Where(x => x.PhoneNumber.Contains(phone));
            //    searchPerformed = true;
            //}


            if (searchPerformed)
            {
                // return search results
                //return View(cust.ToList());
                return View(cust2);
            }
            else
            {
                // return empty list
                return View(cust2);
            }

        }


        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.FrequencyID = new SelectList(db.Frequencies, "id", "description", customer.FrequencyID);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,phoneNumber,CarModel,CarMake,CarCapacity,CarAverage,PerHourRate,FreeFrom,FreeTo,Available,FrequencyID,street1,street2,city,state")] Customer customer)
        {
            bool custsameasuser = db.Customers.Any(x => x.id == customer.id && x.email == User.Identity.Name);
            if(custsameasuser==false)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var prcust = db.Customers.FirstOrDefault(x => x.id == customer.id);
                customer.longitude = prcust.longitude;
                customer.latitude = prcust.latitude;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FrequencyID = new SelectList(db.Frequencies, "id", "description", customer.FrequencyID);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Details(Customer model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var message = new MailMessage();
        //        var rep1 = User.Identity.Name;
        //        message.To.Add(new MailAddress(rep1));  // replace with valid value 
        //        message.To.Add(new MailAddress(model.email));
        //        message.From = new MailAddress("kundamn91@gmail.com");  // replace with valid value
        //        message.Subject = "Your email subject";
        //        message.Body = "Hi"; //string.Format(body, model.FromName, model.FromEmail, model.Message);
        //        message.IsBodyHtml = true;

        //        using (var smtp = new SmtpClient())
        //        {
        //            var credential = new NetworkCredential
        //            {
        //                UserName = "kundamn91@gmail.com",  // replace with valid value
        //                Password = "2016@Fall"  // replace with valid value
        //            };
        //            smtp.Credentials = credential;
        //            smtp.Host = "smtp.gmail.com";
        //            smtp.Port = 587;
        //            smtp.EnableSsl = true;
        //            await smtp.SendMailAsync(message);
        //            return RedirectToAction("Sent");
        //        }
        //    }
        //    return View(model);
        //}

        //public ActionResult Sent()
        //{
        //    return View();
        //}
        // POST: Customers/MailSend
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Details()
        {
            var cust = db.Customers.Single(c => c.id == newid);
            var smtpClient = new SmtpClient();
            var msg = new MailMessage();
            msg.To.Add(User.Identity.Name);
            msg.Subject = "Thank you for your interest!";
            msg.Body = "Thank you for expressing your interest in renting car through KAM Car-Rentals." +
                " Please go ahead and contact owner through following details"+ " "+ 
                "Phone Number"+ cust.phoneNumber+ " "+
                "Email Address" + cust.email
                ;

            smtpClient.Send(msg);

            var smtpClient2 = new SmtpClient();
            var msg2 = new MailMessage();
            msg2.To.Add(cust.email);
            msg2.Subject = "You have been contacted through KAM Car-Rentals!";
            msg2.Body = "Hello! An user has expressed interest in your car through KAM Car-Rentals. And he will contact you soon" +
                " Alternatively if you wish to contact the user, you can email on following address" + User.Identity.Name
                ;

            smtpClient.Send(msg2);

            return RedirectToAction("Index");
        }

        public ActionResult Sent()
        {
            return View();
        }
        public string OpenModelPopup()
        {
            //can send some data also.
            return "<h1>This is Modal Popup Window</h1>";
        }

        //public async Task sendEmail(string sEmail, string service, int id)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://emailweb.azurewebsites.net/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        Email email = new Email();
        //        email.serviceEmail = sEmail;
        //        email.customerEmail = User.Identity.Name;
        //        email.serviceName = service;
        //        HttpResponseMessage response = await client.PostAsJsonAsync("api/Mail", email).ConfigureAwait(false);
        //        if (response.IsSuccessStatusCode)
        //        {

        //        }

        //    }

        //}

    }
}
