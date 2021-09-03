# GoussanBlogSolution
 
 A Simple Media Web App that works using a .NET 6.0 (Preview) Backend API along with React.js frontend Client Application

 ## GoussanBlog

React Javascript Frontend Application/Website that handles calls to backend API and renders it to Users
<br />
Documentation: [Link To Documentation](GoussanBlog/README.md)

### Planned Features
* File Validation on Video Upload to ensure valid video file provided for encoding
* Image Upload
* Image Upload Validation to ensure valid Image file
* Blog post Creation
* Blog post validation to ensure blog post does not contain anything malicious like file insertion or malicious links
* Render Videos in Cards in a sidescrolling slide
* Render Images in Cards in a sidescrolling slide
* Render Blog Posts in a sidescrolling slide
* Preview Blog Posts before creation
* Insertion of Images and Videos into Blog Posts
* View Uploads/Blog Posts by Users
* Sort Video by User, Upload Date or Recommended
* Sort Images by User, Upload Date or Recommended
* Sort Blog Posts by User, Upload Date or Recommended
* Limit amount of Videos fetched at a time to reduce load time
* Limit amount of Videos rendered at a time to reduce load time and resource consumption
* Limit amount of Images fetched at a time to reduce load
* Limit amount of Images rendered to reduce load time and resource consumption
* Create a Subscribe to Users button
* Create a theme for the whole project
* Redo the styling of all the components
* Make the Website responsive

## GoussanBlogData

.NET 6.0 (Preview) Backend API that handles interaction between Client App and Azure Services, handling password encryption and token verification. Handles creation of containers, 
<br />
Documentation: [Link To Documentation](GoussanBlogData/README.md)

### Planned Features
* Image Controller to handle CRUD operations for Images received by the front end application
* Blog Controller to handle CRUD operations for Blog Posts received by the front end application
* Write up documentation for the Swagger Docs to give a easier overview and ways to test out the API
* Refactor the Code to remove unused references
* Redo Models in Use to remove unused properties

## GoussanFunction

Azure Functions App that events received from Azure Services to perform changes on data in Cosmos DB depending on their state. Like Encoding Tasks finishing or User Creation and sending a verification email to User upon completing registration.
<br />
Documentation: [Link To Documentation](GoussanFunction/README.md)

### Planned Features
* Send out Emails to users subscribed to other Users upon new uploads or blogs from users
* Create Email Templates to be used instead of hard coding the email sent



