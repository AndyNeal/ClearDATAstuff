# Software Engineer Assessment Test: 
You will receive a string as input, potentially a mixture of upper and lower case, numbers, special characters etc. The task is to determine if the string contains at least one of each letter of the alphabet. Return true if all are found and false if not. Write it as a RESTful web service (no authentication necessary) in any language/framework you choose and document the service. Please bring your laptop on the day of your interview to present your information.

## Live version
I went ahead and spun this up on an EC2 instance I had handy

[SwaggerUI](https://validator.andy-neal.com/swagger/index.html)

[OpenAPI json](https://validator.andy-neal.com/swagger/v1/swagger.json)

# Considerations:
## Verb choice:
Being that this is meant to be RESTful, the HTTP verb/method we’ll use should reflect the action we’re taking.  So that *Should* be a GET here as we’re get retrieving information, that said this is a problem I’ve run into before and wanted to bring up.  Passing data in the request body on a GET request is not proper according to RFC, it is certainly done, and this codebase can function that way, but not everything will play nice (interestingly Swagger/OpenAPI will create the json for it here but SwaggerUI will choke).  Given the option of implementing a potentially unsupported operation on what I assume we are meaning to be a public API I included that function as a POST here, more on that below.
## Inputs:
The obvious way to provide the input string to this webservice is as a parameter and that may well be adequate for this application (and would facilitate caching).  The input value could also be provided in the path but I’m not seeing that as appropriate here.  I think a key thing to note is that if this is meant to do something like password complexity validation – where that input value may be sensitive information including the input value in the URL simply wouldn’t be acceptable, so this webservice is accepting a JSON object in the body as well – for the universal acceptance reason stated above this is as a POST.  Lastly you may end up with a position where your input string contains characters that would cause misinterpretation of input values, and consumers really should be able to escape/url-encode those characters so that it isn’t an issue, if only to point out the issue and another solution we are also accepting the input value here as base64 encoded UTF-8.
## Endpoint naming:
Not having context all we can do is choose reasonable names though I do generally prefer to version everything, even if they are unlikely to ever progress beyond v1 – Really just to give the option to roll out updates without needing to alter every consumer related at the same time, really given a publicly available webservice that isn’t an option (and violates open/closed anyway).  We do need to be careful about versioning becoming a vector to couple applications.  I left behind the default "/api" in the route as I have been using that lately to host webservices on the same instance as a single page application and then adjust routing to send everything not under /api to the SPA - this gets around CORS but may not be required.
## Responses:
In this specific case I think we’re well covered with simply using the status code and a simple return, that still allows the webservice to report error status and messages.  The POST endpoint does return a JSON object, often I have simply done that as a stylistic choice.  And given a more complex set of requirements you may need a more complex error return.
## CORS:
Probably not an issue for this application but I tend to include the configuration out of habit.
## Documentation:
This webservice is using Swagger/ OpenAPI, as a general statement the documentation is self-generating this way.  There are some things to note to make certain you are coding so that the documentation is generated accurately, but certainly not enough to omit tooling like this.  (Specifically it does not identify potential response codes unless specifically defined, ex: using the [Require] attribute will add a potential 400 code, but Swagger doesn’t recognize that unless you also specify [ProducesResponseType(StatusCodes.Status400BadRequest)]).
## Logic:
The obvious choice here is to use a regular expression, the crux of the application logic is analyzing a string against a pattern – and there’s a whole markup for that!  This is the most maintainable and extensible approach in my opinion (though I will concede that a regex of 26 lookaheads is a little rough).  I added a few other approaches and a benchmark endpoint for giggles – and I find that is often interesting (though not always relevant).  
## Configuration:
Here we are drawing configuration information off the appsettings.json file (the regex, required character list, CORS origins) with a deployment pipeline this is also where I have done transformations, in this case as it stands that would probably only be the CORS Origins – more sophisticated webservices would often environment variables as they proceed up through a CI/CD pipeline.
## Testing:
There isn’t really anything here, but this is why I broke out the “logic” functions into a repository that comes back to the controllers via Dependency Injection, we could define unit tests against the repository and have automation validate that functionality – however at it’s current state I would be question the value of it – the “real” endpoints are just using the regex wrapper, and so tests defined in here would either validate that Regex.Match() still works or behavior that is really coming from the regex in the config.  To get on a soapbox I think “bad” testing is worse than no testing, this can be a fantastic approach to improve code quality and speed up delivery, but badly executed they erode trust and waste time (Anecdotally my most ridiculous example was more than 700 unit tests that validated non-nullable objects were in fact not null.  It had 100% code coverage!  Lots of it didn’t work, but the code coverage!)
## Authentication/Authorization:
Normally something to be addressed, which was specifically called out as unneeded here – I bring it up because I think this should be included in the initial requirements, retrofitting multi-tenancy access control to applications is not my favorite thing in the world.
## Logging:
Something that would normally need to be addressed but I really just skipped it given the requirements.

## Todo
This is all in regard to the dotnet version, I may still spin up additional versions using other languages/frameworks.
