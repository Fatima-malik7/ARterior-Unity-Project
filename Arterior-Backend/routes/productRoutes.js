const express = require('express');
const router = express.Router();
const Product = require('../models/Product');

// POST product from Unity
router.post('/', async (req, res) => {
  try {
    const { name, description, imageUrl } = req.body;
    const product = new Product({ name, description, imageUrl });
    await product.save();
    res.status(201).json({ message: 'Product saved!' });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// GET all products
router.get('/', async (req, res) => {
  const products = await Product.find();
  res.json(products);
});

module.exports = router;
