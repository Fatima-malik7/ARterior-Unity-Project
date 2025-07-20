// seed.js
require('dotenv').config();
const mongoose = require('mongoose');
const FurnitureItem = require('./models/FurnitureItem');

// Connect to MongoDB
mongoose.connect(process.env.MONGO_URI, {
  useNewUrlParser: true,
  useUnifiedTopology: true
})
  .then(async () => {
    console.log("✅ Connected to MongoDB!");

    // Optional: Clear existing items (only if needed)
    // await FurnitureItem.deleteMany({});
    
    // Insert sample furniture items
    const insertedItems = await FurnitureItem.insertMany([
      {
        name: "Modern Grey Sofa",
        price: "PKR14,999/-",
        imageUrl: "http://localhost:5000/images/light-grey-sofa.jpg",
        category: "Living"
      },
      {
        name: "Green Sofa Chair",
        price: "PKR17,999/-",
        imageUrl: "http://localhost:5000/images/green-sofa-chair.jpg",
        category: "Living"
      }
    ]);

    console.log("✅ Inserted Furniture Items:");
    console.table(insertedItems.map(item => ({
      id: item._id.toString(),
      name: item.name,
      category: item.category
    })));

    // Log connected DB name
    console.log("📁 Using Database:", mongoose.connection.name);

    process.exit();
  })
  .catch(err => {
    console.error("❌ MongoDB connection failed:", err.message);
    process.exit(1);
  });
