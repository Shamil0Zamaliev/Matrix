using Matrix;


squarematrix m3 = new (3);
m3.AutoFillMatrix();
m3.Show();
Console.WriteLine(m3.Determine());
squarematrix m4 = m3.Inverse();
m4.Show();
matrix m5 = m3 * m4;
m5.Round();
m5.Show();




