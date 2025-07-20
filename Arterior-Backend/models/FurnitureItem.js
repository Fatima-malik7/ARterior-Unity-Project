const mongoose = require('mongoose');

const FurnitureItemSchema = new mongoose.Schema({
  name: String,
  price: String,
  imageUrl: String,
  category: String
});

module.exports = mongoose.model('FurnitureItem', FurnitureItemSchema);
