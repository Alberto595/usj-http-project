# 👩🏻‍💻 HTTP group project

## 🗺️ Overview

This group project statemnt is part of the course **Networks & Communications 2** of **Universidad San Jorge** of Zaragoza. All contents are original.

### 🧐 Intro

During our course, a key protocol we cover is the Hypertext Transfer Protocol (HTTP). HTTP is an application layer protocol within both the OSI and TCP/IP models, primarily used for transmitting hypermedia documents, such as HTML. It forms the foundation of data communication for the World Wide Web, operating on a client-server model that facilitates the fetching of resources, like web pages, from a server to a client, typically a web browser.

The objective of the group project is to achieve a deep understanding of the HTTP protocol. This includes everything from its implementation at the lowest level to advanced details such as caching, authentication, or the implementation of REST APIs.

In this group project, we aim to implement a simplified version of the HTTP protocol from scratch. Students are encouraged to select the programming language and/or framework of their choice, as well as any development tools they deem most appropriate for the task. The primary goal of this exercise is to offer practical experience in building network applications and to deepen the understanding of HTTP's role and functionality in web development.

### 🧰 Project outcome

The project involves developing from scratch, using only transport layer network libraries, a basic HTTP client and server. This client and server should operate between themselves but also, by adhering to the protocol standard, interoperate with any other "real" client or server.

Unless the optional GUI feature is implemented, the client will be a command-line program, more or less interactive depending on the optional features decided to be implemented. In summary, a program capable of launching HTTP messages and processing the response messages to these requests is expected.

On the other hand, the HTTP server will be a program capable of receiving HTTP requests and reacting to them, updating its internal state and responding with various web objects as necessary. The HTTP server is a program that must always be available to receive requests, whether from one or more clients, sequentially or concurrently, and whether some are unsuccessful and others are successful.

Both the client and server can be developed in the programming language and/or framework that the group decides. The only technological restriction is that the use of any network library from an OSI layer higher than the transport layer is not allowed. That is, libraries that interact with TCP sockets can (and should) be used, but the use of libraries that work directly with HTTP requests, either sending or receiving them, is not allowed. The task consists, in fact, of achieving the implementation of such a library. Some examples of the type of libraries not allowed would be Axios or Express in the NodeJS ecosystem, SpringBoot or OkHttp in the Java ecosystem, or libcurl in the C++ ecosystem.

The ideal programming language is the one with which the group is most familiar. However, due to the appropriate level of abstraction it offers, NodeJS is suggested only as a recommendation.

### 🚚 Delivery

The project delivery consists of two elements:

- The code repository in which the practice has been developed, with its original change history and version control
- A technical report of a maximum of 10 pages explaining in as much detail as desired the most important decisions made, challenges overcome, group task distribution, and work methodology followed.

### 🔎 Evaluation

The project evaluation will be based on a maximum possible grade chosen by the group itself. This grade will be determined by the optional features implemented. The score that these features will add varies depending on the number of group members, between two and six, with the recommended option being to form groups of between three and five students. In addition, there will be a series of mandatory features that must be implemented in any case to pass the project.

The evaluation will be based on the correctness of the technical report and the code available in the latest version of the code repository, as well as a live demo that the group will have to perform. During this demo, the professor may discretionally ask students any questions deemed appropriate. The live demo can be scheduled at any time the group wishes, but no later than **May 24, 2024**. The technical report and the code repository must be delivered at that time.

Original, very high-quality code is expected, following good practices, appropriate design patterns, good formatting, and good naming. This will be especially taken into account during the evaluation.

It is allowed to use generative AI tools such as ChatGPT for information seeking and research, but if done, it must be specified. Both the report and the code will be subjected to a heuristic check to determine if it has been generated with generative AI. If so and not explicitly indicated, it will result in the lab being graded with "0".

## 🛂 Mandatory features

**All** of the following features are mandatory for **all** groups, regardless of their number of members. The maximum score they can contribute will vary based on the number of members as detailed in the attached table.

For proper project development, it is essential to implement the mandatory features **before** starting with the optional ones.

### 🚢 HTTP Client

The program that interacts as an HTTP client must be able to execute the following features:

- Send HTTP requests, in a way that:
  - It is possible to choose the URL to which the request will be sent
  - Use any available HTTP verb in the request (GET, HEAD, POST, PUT, DELETE)
  - Automatically add the necessary headers to the request so that it can be processed correctly
  - Add any other arbitrary header desired by the user
  - Specify the body of the request
- Receive and display on screen the response message of the sent request
- Inform about the request status
- Be able to send successive requests, i.e., to send a second request it is not necessary to restart the program

### 🏗️ HTTP Server

The HTTP server must be able to do the following:

- Support, at least, the following endpoints, when they are correctly called (correct verb, correct headers...):
  - An endpoint that returns static content (e.g., a static HTML file)
  - An endpoint that adds a new resource to the server according to the specified payload
  - An endpoint that allows viewing a list of resources
  - An endpoint that allows modifying a resource
  - An endpoint that allows deleting a resource
- Return the appropriate error codes if the endpoints are not invoked correctly
- Attend to multiple requests concurrently
- Offer minimal configuration that allows choosing on which port the server starts
- It is not necessary for the resources to be persisted; they can be managed in memory

#### 💬 Clarification on endpoints and resources

The nature of the resources managed by the server is up to the group's discretion, as well as the static contents it serves. For example, a group that likes kittens could focus its server on this theme and serve a simple HTML web page about a cat shelter in `/adoption.html` and manage a series of kitten resources (e.g., `{name: "Hercules", breed: "European", age: 3}`) through the `/cats` endpoint, so that it is possible to register new cats for adoption, delete them when they are adopted, modify them, and list them.

## 🚀 Optional features

The following features are optional and allow increasing the maximum possible grade that the student can opt for. **Always** the basic features must be finished before starting with the advanced ones.

### 🔑 Authentication with API key

**Difficulty:** Low

Implement basic authentication based on an API key. It consists of making the HTTP server only accept HTTP requests from those clients that include a key in a specific header of their requests. If not done, those requests must be rejected with the appropriate error code. The API key must be configurable in both client and server without the need to recompile either program.

### 🔐 Authentication with login flow

**Difficulty:** Medium

Implement a complete authentication flow. In this case, the server will have to manage (create, modify, delete) a series of `User` resources storing their username and password. The server will have to support a login endpoint where the client can pass its username and password to obtain a session token in return. Subsequent client requests can authenticate by including this token in a header, being rejected otherwise.

### 📸 Sending and receiving multimedia files

**Difficulty:** Medium

Enable the possibility for the client and server, following the MIME standard, to send and receive multimedia content such as images. For example, resources may now include PNG images.

### ☢️ TLS

**Difficulty:** High

Support the TLS protocol, so that traffic between client and server travels encrypted over the TCP connection. It requires managing the TLS handshake, the exchange of asymmetric keys, and the generation of ephemeral symmetric keys correctly.

### 📓 Logging

**Difficulty:** Low

Write in a file all the activity that occurs and the requests received on the server, including different levels of logging, timestamps, etc.

### 🧪 Automated Testing

**Difficulty:** Medium

Use automated testing tools such as Jest or JUnit to automatically test the endpoints exposed by the server. For example, creating a series of cats in `/cats` and checking that these cats can then be recovered.

### ☁️ Deployment on a real server

**Difficulty:** Low

Upload the HTTP server to a publicly available machine and have it function from there. Solutions such as virtual machines or serverless environments can be used.

### ⚙️ Refactor with HTTP framework

**Difficulty:** Low

Implement a second version of the HTTP server using high-level HTTP libraries such as Express this time. Demonstrate that it is capable of interoperating with the original client.

### 💾 Conditional GET with cache

**Difficulty:** Medium

Implement a mechanism of conditional GET that allows storing a series of resources in the client's local cache and reloading them only if they have been modified since the last time, thereby reducing traffic between client and server.

### 🎨 GUI for the client

**Difficulty:** Medium

Add a graphical user interface to the client, so that requests can be visually configured before being sent, as well as their responses.

### 🍪 Cookies

**Difficulty:** Low

Implement a system of persistent cookies in the client, which can be set from the server side and automatically sent back transparent

ly to the user in all subsequent requests.

### 🎰 Advanced CRUD

**Difficulty:** Low

Complicate the basic CRUD proposed by managing more resources and establishing relationships between them, nesting, etc.

### 🧠 Anything else

**Difficulty:** ???

It is possible to propose any other optional features to the professor. A score will be agreed upon with the group of students based on their number of members.

## 📝 Grading of features

| Feature                      | 2 students | 3 students | 4 students | 5 students | 6 students |
| ---------------------------- | ---------- | ---------- | ---------- | ---------- | ---------- |
| Mandatory features           | +8 pts.    | +7 pts.    | +6 pts.    | +5 pts.    | + 4 pts.   |
| API key                      | +1 pts.    | +0.8 pts.  | +0.6 pts.  | +0.4 pts.  | + 0.3 pts. |
| Login flow                   | +3 pts.    | +2.7 pts.  | +2.3 pts.  | +2 pts.    | + 1.8 pts. |
| Multimedia messages          | +2 pts.    | +1.7 pts.  | +1.3 pts.  | +1 pts.    | + 0.8 pts. |
| TLS                          | +4 pts.    | +3.6 pts.  | +3.3 pts.  | +3 pts.    | + 2.8 pts. |
| Logging                      | +1 pts.    | +0.8 pts.  | +0.6 pts.  | +0.4 pts.  | + 0.3 pts. |
| Automated testing            | +2 pts.    | +1.7 pts.  | +1.3 pts.  | +1 pts.    | + 0.8 pts. |
| Real server deployment       | +1 pts.    | +0.8 pts.  | +0.6 pts.  | +0.4 pts.  | + 0.3 pts. |
| Refactor with HTTP framework | +2 pts.    | +1.7 pts.  | +1.3 pts.  | +1 pts.    | + 0.8 pts. |
| Conditional GET              | +2 pts.    | +1.7 pts.  | +1.3 pts.  | +1 pts.    | + 0.8 pts. |
| Client GUI                   | +2 pts.    | +1.7 pts.  | +1.3 pts.  | +1 pts.    | + 0.8 pts. |
| Cookies                      | +1 pts.    | +0.8 pts.  | +0.6 pts.  | +0.4 pts.  | + 0.3 pts. |
| Advanced CRUD                | +1 pts.    | +0.8 pts.  | +0.6 pts.  | +0.4 pts.  | + 0.3 pts. |

## 🔁 Proposed work plan for mandatory features

The project is broad and can be challenging, but it is entirely solvable by students of our degree and represents a great opportunity to learn new things. For those groups that feel somewhat overwhelmed and unclear on how to start the project, this work structure is proposed:

1. Develop a "client library" that allows HTTP requests to be made in a simple and clean way, without getting into implementation details. That is, to create a series of functions that can be invoked in a way similar to this:

   ```
   const response = myClientLib.request('GET', 'http://localhost/cats, headers: {'key': 123}, body: {}')
   ```

   To validate that this first step has been successfully completed, tools like [Beeceptor](https://beeceptor.com) can be very useful.

2. Define a "server library," analogous to the "client library." In this case, we need to achieve a suite of functions that allow us to abstract to a certain extent from the reception of requests. For example, something along the lines of:

   ```
   myServerLib.on('get', '/cats', {
       ...
   });
   ```

   This part can be easily validated with tools like [Insomnia](https://insomnia.rest), [Postman](https://www.postman.com) or simply [cURL](https://curl.se).

3. Implement the HTTP client as an interactive CLI that uses the library from the first step to be able to launch dynamic requests. Again, Beeceptor is an excellent ally for debugging.

4. Enable in our HTTP server, using our "server library," a first endpoint that statically returns an HTML file read from disk.

5. Implement the CRUD in a basic way by adding more endpoints and simple in-memory persistence

6. Address possible error cases
