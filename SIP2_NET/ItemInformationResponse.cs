/**************************************************************
 * 
 *  (c) 2014 Mark Lesniak - Nice and Nerdy LLC
 *  
 * 
 *  Implementation of the Standard Interchange Protocol version 
 *  2.0.  Used to standardize queries across multiple database
 *  architectures.  
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 *  
 * 
**************************************************************/

using System;

namespace SIP2
{
    public class ItemInformationResponse
    {
        public string DueDate { get; set; }
        public string Title { get; set; }
        public string ItemBarcode { get; set; }
        public string Message { get; set; }

        public int CirculationStatus { get; set; }
        public int SecurityMarker { get;  set; }
        public int FeeType { get;  set; }
        public int? HoldQueueLength { get; set; }
        public string MediaType { get; private set; }
        public string PermanentLocation { get; private set; }

        public static ItemInformationResponse Parse(string ItemResponse)
        {
            var iir = new ItemInformationResponse();
            string[] item_data = ItemResponse.Split(new Char[] { '|' });

            if (ItemResponse.Substring(0,2) != "18") throw new Exception("Invalid Item Information Response");


            if (int.TryParse(ItemResponse.Substring(2, 2), out var cs))
                iir.CirculationStatus = cs;
            else 
                throw new SIP2.InvalidParameterException("Invalid Circulation Status");

            if (int.TryParse(ItemResponse.Substring(4, 2), out var sm))
                iir.SecurityMarker = sm;
            else
                throw new SIP2.InvalidParameterException("Invalid Security Marker");

            if (int.TryParse(ItemResponse.Substring(6, 2), out var tf))
                iir.FeeType = tf;
            else
                throw new SIP2.InvalidParameterException("Invalid FeeType");

            foreach (string element in item_data)
            {
                if (!String.IsNullOrEmpty(element))
                {
                    // Due Date               
                    if (element.Substring(0, 2).ToUpper() == "AH") iir.DueDate = element.Substring(2);

                    // Item title
                    if (element.Substring(0, 2).ToUpper() == "AJ") iir.Title = element.Substring(2);

                    // Item barcode
                    if (element.Substring(0, 2).ToUpper() == "AB") iir.ItemBarcode = element.Substring(2);

                    // Institution id
                    if (element.Substring(0, 2).ToUpper() == "AQ") iir.PermanentLocation = element.Substring(2);

                    // Screen message
                    if (element.Substring(0, 2).ToUpper() == "AF") iir.Message = element.Substring(2);

                    // Hold Queue Length
                    if (element.Substring(0, 2).ToUpper() == "CF" && int.TryParse(element.Substring(2), out var hql)) iir.HoldQueueLength = hql;

                    if (element.Substring(0, 2).ToUpper() == "CK") iir.MediaType = element.Substring(2);


                }
            }

            return iir;
        }
    }
}
