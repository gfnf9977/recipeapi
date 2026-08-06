# 🍽️ Culinary CRM (Recipe Manager)

A lightweight, fast, and responsive web application for managing your personal recipe collection. The project allows you to conveniently structure ingredients, store video instructions (TikTok/YouTube), maintain a cooking diary, and leave notes for specific dish variations.

🌍 **Live Demo:** [View Project in Action](https://recipeapi-hm5q.onrender.com/)

---

## ✨ Key Features

- **Instant Search:** Filter recipes by name or ingredients in real-time on the client side.
- **Smart Media Integration:** Automatically fetch thumbnails from videos (TikTok/YouTube) via oEmbed API.
- **Cooking Tracking:** Mark prepared dishes, add your own photo reports, and view them in full-screen mode (Lightbox).
- **Source Variability:** Add notes to each individual video source under a single recipe (e.g., *"the author uses cream instead of milk here"*).
- **Ingredient Sorting:** Dynamic division into required components and optional ingredients.

---

## 🛠 Tech Stack

- **Backend:** C#, ASP.NET Core (Minimal APIs), Entity Framework Core
- **Frontend:** Vanilla JavaScript, HTML5, Tailwind CSS
- **Database:** PostgreSQL (via [Neon.tech](https://neon.tech/))
- **Cloud Storage:** [ImgBB API](https://api.imgbb.com/)

---
---
## 🏗 Architectural Solutions

To ensure speed and reliability when deploying on free cloud servers, the project implements two key solutions:

1. **Serverless Database (Neon PostgreSQL)**
   Instead of local or temporary solutions, the database is hosted on the Neon cloud platform. This ensures data "immortality", independence from backend server restarts (e.g., on Render), and provides fast access thanks to the modern compute-storage separation architecture.

2. **Media File Storage Optimization (Zero-Footprint Storage)**
   To avoid overloading the database and exhausting memory limits with heavy user photos, a direct upload mechanism is implemented:
   - Images are sent to **ImgBB** servers directly from the client.
   - Only a lightweight URL string (< 1 KB) is stored in PostgreSQL.
   - This allows scaling the project to thousands of recipes while keeping the database maximally compact and fast.

---
---
## 🚀 Installation & Local Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/recipe-crm.git
   ```
2. Navigate to the project folder and configure your PostgreSQL connection string. Set the environment variable:
   ```bash
   set DB_CONNECTION_STRING=postgres://user:password@host:port/dbname
   ```
3. Run the project (database and tables are created automatically via `EnsureCreated`):
   ```bash
   dotnet run
   ```
