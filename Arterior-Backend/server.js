const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
require('dotenv').config();

const app = express();
app.use(cors());
app.use(express.json());

app.use('/images', express.static('images'));


// Routes
const productRoutes = require('./routes/productRoutes');
app.use('/api/products', productRoutes);

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
.catch((err) => console.error('MongoDB error:', err));
