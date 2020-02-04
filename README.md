# Effective AJAX calls with jQuery and ASP.NET Core

> In this example, I want to show a possible problem while doing fast ajax calls and how to solve it.
> I made a little code example that simulates an autocomplete texbox with names.
<br/>

#### Table of Contents
[How it works?](#how-it-works)  
[Let's test](#lets-test)  
[Problem](#lets-test)  
[Solution](#lets-test)
<br/>

## How it works?
Really simple:
<br/>
When we input some value into the textbox, it calls the ajax function and it sends a request to the server asynchronously.
<br/>

### Client-side AJAX function

```javascript
$.ajax({
    type: "POST",
    url: "/main/getnames",
    data: JSON.stringify(parameters),
    contentType: 'application/json',
    success: function (data, textStatus, jqXHR) {
        $("#label").html("");
        for (var i = 0; i < data.length; i++) {
            $("#label").append(data[i] + "<br>");
        }
    },
    error: function () {
        alert("Error!")
    }
});
```
### Server-side Controller

```c#
[HttpPost("getnames")]
public JsonResult GetNames([FromBody] Pattern pattern)
{
    return Json(names.Where(str => str.ToUpper().StartsWith(pattern.text.ToUpper())));
}
```

That means we are able to input as much as we want, ajax will send the corresponding requests as we write (non-blocking), and the server is able to process several requests concurrently and we get responses as they are processed on the server.

## Let's test:

Let's try to write *l*. We will get *Luís*, *Laura* and *Luisa*:
<br/><br/>
![alt text](https://github.com/oscarsolerfollana/AJAX-jQuery-request-response-match/blob/master/ReadmeContent/test_l.PNG?raw=true)
<br/><br/>
And if we write *lu*, we get *Luís* and *Luisa*:
<br/><br/>
![alt text](https://github.com/oscarsolerfollana/AJAX-jQuery-request-response-match/blob/master/ReadmeContent/test_lu.PNG?raw=true)

So far, so good.

## Problem:

The problem comes in some cases, when we try to do many inputs (or selections in the case of a combo), in other words, many requests in a short period of time. Due to asynchronous nature of our calls, if two requests are thrown almost in the same time, or there are network problems, latency or packets loss, the second response may arrive before the first one (the order is not garanteed).

Bescause of that, we may have this result (real result trying by myself):
<br/><br/>
![alt text](https://github.com/oscarsolerfollana/AJAX-jQuery-request-response-match/blob/master/ReadmeContent/bug.PNG?raw=true)

In this case, the response related to letter *u* (second request) arrives first. Then, letter *l* response arrives and finally, the result does not match with our text to find.

## Solution:

To resolve this, here goes a little trick:

We must always check the response, in order to match with what we have in our checkbox:

We associate a unique identifier with each call:
<br/>

```javascript
$.ajaxPrefilter(function (options, originalOptions, jqXHR) {
    id++;
    jqXHR.id = id;
});
```

And inside the ajax function, we check if the response identifier matches with the last request identifier:
<br/>

```javascript
success: function (data, textStatus, jqXHR) {
    if (jqXHR.id == id) {
        $("#label").html("");
        for (var i = 0; i < data.length; i++) {
            $("#label").append(data[i] + "<br>");
        }
    }
},
```

 That way, we ensure the results only show if the ajax response match with the last request, hence with the current input.

## License

- **[Copyright 2015 © Óscar Soler Follana](https://github.com/oscarsolerfollana/Effective-AJAX-calls-with-jQuery-and-ASP.NET-Core/blob/master/LICENSE/license.md)**
