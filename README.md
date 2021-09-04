# GoussanBlogSolution
 
 A Simple Media Web App that works using a .NET 6.0 (Preview) Backend API along with React.js frontend Client Application

 ## GoussanBlog

React Javascript Single Page Application/Website that handles calls to backend API and renders it to Users
<br />
Documentation: [Link To Documentation](GoussanBlog/README.md)

### Current Features
* User Registration and Login
* User Confirmation
* Video Upload
* Video Streaming

### Planned Features
* File Validation on Video Upload to ensure valid video file provided for encoding
* Image Upload
* Image Upload Validation to ensure valid Image file
* Blog post Creation
* Blog post validation to ensure blog post does not contain anything malicious like file insertion or malicious links
* Render Videos/Images/Blog Posts/Blogs? in Cards in a sidescrolling slide
* Preview Blog Posts before creation
* Insertion of Images and Videos into Blog Posts
* View Uploads/Blog Posts by Users
* Sort Videos/Images/Blog Posts by User, Upload Date or Recommended
* Limit amount of Videos/Images/Blog Posts fetched/rendered at a time to reduce load time
* Create a Subscribe to Users button
* Create a theme for the whole project
* Redo the styling of all the components
* Make the Website responsive

## GoussanBlogData

.NET 6.0 (Preview) Backend API that handles interaction between Client App and Azure Services, handling password encryption and token verification. Handles creation of containers, 
<br />
Documentation: [Link To Documentation](GoussanBlogData/README.md)

### Current Features
* User Controller that handles User CRUD operations
* JWT Utilities Verifies JWT Token and extract user information from database
* Video Controller that handles CRUD operations related to Videos received by the API
* Media Services that consumers Video File and encodes video and stores video in Azure and provide VoD and streaming capabilities
* CosmosDb Services Service handler for operations related to Cosmos DB

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



