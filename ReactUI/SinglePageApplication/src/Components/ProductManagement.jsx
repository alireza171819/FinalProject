import { useCallback, useEffect, useMemo, useState } from 'react';
import {
  Button,
  Card,
  CardHeader,
  CardFooter,
  Field,
  Input,
  Spinner,
  Text,
  Textarea
} from '@fluentui/react-components';
import { Add24Regular, ArrowClockwise24Regular } from '@fluentui/react-icons';
import { addProduct, fetchProducts } from '../Services/productService';

const initialFormState = {
  title: '',
  description: '',
  price: ''
};

const ProductManagement = () => {
  const productsApiUrl = "https://localhost:7017/product/GetAll";
  const addProductApiUrl = "https://localhost:7017/product/create";

  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [formState, setFormState] = useState(initialFormState);
  const [submitting, setSubmitting] = useState(false);
  const [feedback, setFeedback] = useState(null);

  const currencyFormatter = useMemo(
    () => new Intl.NumberFormat(undefined, { style: 'currency', currency: 'USD' }),
    []
  );

  const loadProducts = useCallback(async () => {
    if (!productsApiUrl) {
      setError('The VITE_PRODUCTS_API_URL environment variable is not configured.');
      setLoading(false);
      return;
    }

    try {
      setLoading(true);
      setError('');
      const items = await fetchProducts(productsApiUrl);
      setProducts(items);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }, [productsApiUrl]);

  useEffect(() => {
    loadProducts();
  }, [loadProducts]);

  const handleInputChange = useCallback((fieldName) => (event, data) => {
    setFormState((current) => ({
      ...current,
      [fieldName]: data.value
    }));
  }, []);

  const isFormValid = useMemo(() => {
    const trimmedTitle = formState.title.trim();
    const price = Number.parseFloat(formState.price);
    const stock = Number.parseInt(formState.stock || '0', 10);

    return (
      trimmedTitle.length > 0 &&
      Number.isFinite(price) &&
      price >= 0 &&
      Number.isInteger(stock) &&
      stock >= 0
    );
  }, [formState.price, formState.stock, formState.title]);

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (!isFormValid || submitting) {
      return;
    }

    if (!addProductApiUrl) {
      setFeedback({ type: 'danger', message: 'The VITE_ADD_PRODUCT_API_URL environment variable is not configured.' });
      return;
    }

    setSubmitting(true);
    setFeedback(null);

    try {
      const payload = {
        productName: formState.title.trim(),
        productDescription: formState.description.trim(),
        unitPrice: Number.parseFloat(formState.price)
      };

      const createdProduct = await addProduct(addProductApiUrl, payload);

      setProducts((current) => [createdProduct, ...current]);
      setFormState(initialFormState);
      setFeedback({ type: 'success', message: `\`${createdProduct.title}\` was added successfully.` });
    } catch (err) {
      setFeedback({ type: 'danger', message: err.message });
    } finally {
      setSubmitting(false);
    }
  };

  const handleRefresh = () => {
    loadProducts();
  };

  return (
    <div className="container">
      <Card className="product-card border-0 p-4 p-lg-5">
        <CardHeader
          header={
            <div className="d-flex flex-column flex-lg-row justify-content-between align-items-lg-center gap-3">
              <div>
                <Text className="section-title text-uppercase">Product Operations</Text>
                <h1 className="fw-bold mb-0">Product management console</h1>
              </div>
              <Button
                appearance="primary"
                icon={<ArrowClockwise24Regular />}
                onClick={handleRefresh}
                size="large"
                disabled={loading}
              >
                Refresh products
              </Button>
            </div>
          }
        />

        <div className="row g-4">
          <div className="col-12 col-lg-4">
            <section>
              <h2 className="fs-5 fw-semibold mb-3">Add a new product</h2>
              <form className="d-flex flex-column gap-3" onSubmit={handleSubmit}>
                <Field label="Product name" required>
                  <Input
                    value={formState.title}
                    onChange={handleInputChange('title')}
                    placeholder="e.g. Wireless headphones"
                  />
                </Field>

                <Field label="Description">
                  <Textarea
                    value={formState.description}
                    onChange={handleInputChange('description')}
                    resize="vertical"
                    placeholder="Short product summary"
                  />
                </Field>

                <div className="row g-3">
                  <div className="col-12 col-md-6">
                    <Field label="Price" required>
                      <Input
                        type="number"
                        min="0"
                        step="0.01"
                        value={formState.price}
                        onChange={handleInputChange('price')}
                        placeholder="0.00"
                      />
                    </Field>
                  </div>
                </div>

                <Button
                  type="submit"
                  appearance="primary"
                  icon={<Add24Regular />}
                  size="large"
                  disabled={!isFormValid || submitting}
                >
                  {submitting ? 'Saving…' : 'Add product'}
                </Button>

                {feedback && (
                  <div
                    className={`alert alert-${feedback.type} mb-0`}
                    role={feedback.type === 'danger' ? 'alert' : 'status'}
                  >
                    {feedback.message}
                  </div>
                )}
              </form>
            </section>
          </div>

          <div className="col-12 col-lg-8">
            <section>
              <h2 className="fs-5 fw-semibold mb-3">Product catalogue</h2>

              {error && (
                <div className="alert alert-danger" role="alert">
                  {error}
                </div>
              )}

              {loading ? (
                <div className="d-flex justify-content-center py-5">
                  <Spinner size="extra-large" label="Loading products" />
                </div>
              ) : (
                <div className="table-container border rounded-4 overflow-hidden">
                  <table className="table table-striped mb-0">
                    <thead>
                      <tr>
                        <th scope="col">Name</th>
                        <th scope="col" className="d-none d-md-table-cell">
                          Description
                        </th>
                        <th scope="col" className="text-end">
                          Price
                        </th>
                      </tr>
                    </thead>
                    <tbody>
                      {products.length === 0 ? (
                        <tr>
                          <td colSpan="4" className="text-center py-4">
                            <Text weight="semibold">No products were returned by the API.</Text>
                          </td>
                        </tr>
                      ) : (
                        products.map((product) => (
                          <tr key={product.id}>
                            <td>
                              <Text weight="semibold">{product.title}</Text>
                            </td>
                            <td className="d-none d-md-table-cell">
                              <div className="text-truncate" style={{ maxWidth: '320px' }}>
                                <Text>{product.description || '—'}</Text>
                              </div>
                            </td>
                            <td className="text-end">
                              <Text>{currencyFormatter.format(product.price)}</Text>
                            </td>
                            <td className="text-end">
                              <Text>{product.stock}</Text>
                            </td>
                          </tr>
                        ))
                      )}
                    </tbody>
                  </table>
                </div>
              )}
            </section>
          </div>
        </div>

        <CardFooter>
          <Text size={300} className="text-muted">
            Configure the API endpoints through the <code>.env</code> file (see <code>.env.example</code>).
          </Text>
        </CardFooter>
      </Card>
    </div>
  );
};

export default ProductManagement;