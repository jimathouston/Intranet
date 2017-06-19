import 'jest'
const _sum = require('./sum');

describe('stupid frigging addition scheisse', () => {
    it('adds 1 and 2 to equal 3', () => {
        expect(_sum(1, 2)).toBe(3);
    });
    it('adds 2 and 2 to equal 4', () => {
        expect(_sum(2, 2)).toBe(4);
    });
});