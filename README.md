# TransactionQuery.CSharp


* <a href="https://developer.vantiv.com/?utm_campaign=githubcta&utm_medium=hyperlink&utm_source=github&utm_content=gotquestions">Got questions? Connect with our experts on Worldpay ONE.</a>
* <a href="https://developer.vantiv.com/?utm_campaign=githubcta&utm_medium=hyperlink&utm_source=github&utm_content=codingforcommerce">Are you coding for commerce? Connect with our experts on Worldpay ONE.</a>
* Questions?  certification@elementps.com
* **Feature request?** Open an issue.
* Feel like **contributing**?  Submit a pull request.


## Overview

This repository demonstrates using the TransactionQuery Express API call to provide a mechanism to view transactions and transaction detail.  The code was compiled and tested using Microsoft Visual Studio Express 2015 for Windows Desktop.

![TransactionQuery.CSharp](https://github.com/ElementPS/TransationQuery.CSharp/blob/master/TransactionQuery1.PNG)

![TransactionQuery.CSharp](https://github.com/ElementPS/TransationQuery.CSharp/blob/master/TransactionQuery2.PNG)

## Prerequisites

Please contact your Integration Analyst for any questions about the below prerequisites.

* Create Express test account: http://www.elementps.com/Resources/Create-a-Test-Account
* Run a few transactions using the test account above so that there are transactions to report on

## Documentation/Troubleshooting

* Express API:  https://developer.vantiv.com/docs/DOC-1353

## Step 1: Create TransactionQuery XML

```
private string buildTransactionQuery(ConfigurationData configurationData, DateTime startDate, DateTime endDate)
{                           
    XNamespace express = "https://reporting.elementexpress.com";

    XDocument doc = new XDocument(new XElement(express + "TransactionQuery",
                                       new XElement(express + "Credentials",
                                           new XElement(express + "AccountID", configurationData.AccountId),
                                           new XElement(express + "AccountToken", configurationData.AccountToken),
                                           new XElement(express + "AcceptorID", configurationData.AcceptorId)
                                                    ),
                                        new XElement(express + "Application",
                                            new XElement(express + "ApplicationID", configurationData.ApplicationId),
                                            new XElement(express + "ApplicationVersion", configurationData.ApplicationVersion),
                                            new XElement(express + "ApplicationName", configurationData.ApplicationName)
                                                    ),
                                        new XElement(express + "Parameters",
                                            new XElement(express + "TransactionDateTimeBegin", startDate.ToString("yyyy-MM-dd")),
                                            new XElement(express + "TransactionDateTimeEnd", endDate.ToString("yyyy-MM-dd"))
                                                    )
                                               )
                                 );
    return doc.ToString();
}

```

## Step 2: Send a Transaction to Express

This project uses an HttpSender object which basically wraps .net's HttpWebRequest.

```
response = httpSender.Send(request, configurationData.ExpressReportingXMLEndpoint, string.Empty);

```

## Step 3: Receive response and parse

The response is returned as an XML string from Express, the code below loads the XML into an XmlDocument and then parses the XML into an object model which you can find in the Models folder.  This model object is then used to pass to the View to build the transaction table.

```
  var xmlDoc = new XmlDocument();
  xmlDoc.LoadXml(response);
  return ParseResponse(xmlDoc);

```


##### © 2018 Worldpay, LLC and/or its affiliates. All rights reserved.

Disclaimer:
This software and all specifications and documentation contained herein or provided to you hereunder (the "Software") are provided free of charge strictly on an "AS IS" basis. No representations or warranties are expressed or implied, including, but not limited to, warranties of suitability, quality, merchantability, or fitness for a particular purpose (irrespective of any course of dealing, custom or usage of trade), and all such warranties are expressly and specifically disclaimed. Element Payment Services, Inc., a Vantiv company, shall have no liability or responsibility to you nor any other person or entity with respect to any liability, loss, or damage, including lost profits whether foreseeable or not, or other obligation for any cause whatsoever, caused or alleged to be caused directly or indirectly by the Software. Use of the Software signifies agreement with this disclaimer notice.
