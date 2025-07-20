const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
require('dotenv').config();

const app = express();

// Middleware
app.use(cors());
app.use(express.json());

app.use('/images', express.static('images'));


// Routes
const userRoutes = require('./routes/userRoutes');
app.use('/api/users', userRoutes);

const furnitureRoutes = require('./routes/furnitureRoutes');
app.use('/api/furniture', furnitureRoutes);

// Connect DB and Start Server
mongoose.connect(process.env.MONGO_URI, {
  useNewUrlParser: true,
  useUnifiedTopology: true
})
.then(() => {
  console.log('✅ MongoDB connected');
  // app.listen(process.env.PORT, () => {
  app.listen(process.env.PORT, '0.0.0.0', () => {  
    console.log(`🚀 Server running on http://localhost:${process.env.PORT}`);
  });
})
.catch((err) => console.error('❌ MongoDB connection error:', err));
