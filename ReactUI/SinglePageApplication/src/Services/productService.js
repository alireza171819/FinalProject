const ensureArray = (payload) => {
  if (Array.isArray(payload)) {
    return payload;
  }

  if (payload && Array.isArray(payload.products)) {
    return payload.products;
  }

  if (payload && Array.isArray(payload.items)) {
    return payload.items;
  }

  return [];
};

const normalizeProduct = (rawProduct, fallbackSource) => {
  if (!rawProduct && fallbackSource) {
    return normalizeProduct(fallbackSource, null);
  }

  const source = rawProduct ?? {};
  const now = Date.now();

  return {
    id: source.id ?? source.productId ?? `temp-${now}`,
    title: source.title ?? source.productName ?? 'Untitled product',
    description: source.productDescription ?? source.details ?? '',
    price: Number.parseFloat(source.unitPrice ?? source.cost ?? 0) || 0
  };
};

export const fetchProducts = async (apiUrl) => {
  if (!apiUrl) {
    throw new Error('Missing VITE_PRODUCTS_API_URL environment variable');
  }
  const requestOptions = {
    method: "GET",
    headers: {
      "accept": "text/plain",
      "Content-Type": "application/json",
      "Access-Control-Allow-Origin": "*",
      "Access-Control-Allow-Methods" : "GET, POST, OPTIONS",
      "Access-Control-Allow-Headers" : "Content-Type"
    }
  };
  const response = await fetch(apiUrl, requestOptions)
    .catch(error => console.log('error', error));
  console.log(response);
  if (!response.ok) {
    throw new Error(`Failed to fetch products (HTTP ${response.status})`);
  }

  const data = await response.json();
  const products = ensureArray(data).map((product) => normalizeProduct(product));

  if (products.length === 0 && data) {
    // Attempt to normalise a single product payload into a collection.
    const maybeSingle = normalizeProduct(data);
    return [maybeSingle];
  }

  return products;
};

export const addProduct = async (apiUrl, productDraft) => {
  if (!apiUrl) {
    throw new Error('Missing VITE_ADD_PRODUCT_API_URL environment variable');
  }

  const response = await fetch(apiUrl, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(productDraft)
  });

  if (!response.ok) {
    throw new Error(`Failed to add product (HTTP ${response.status})`);
  }

  const createdProduct = await response.json();

  return normalizeProduct(createdProduct, productDraft);
};