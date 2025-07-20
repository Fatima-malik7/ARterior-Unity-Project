const express = require('express');
const router = express.Router();
const FurnitureItem = require('../models/FurnitureItem');

router.get('/', async (req, res) => {
  try {
    const items = await FurnitureItem.find();
    res.json(items);
  } catch (err) {
    res.status(500).json({ message: err.message });
  }
});

module.exports = router;

router.get('/', async (req, res) => {
  const data = await Furniture.find();
  res.json(data);
});
