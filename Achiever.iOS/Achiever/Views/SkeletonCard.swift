//
//  SkeletonCard.swift
//  Achiever
//
//  Created by Tom Brewer on 2/8/24.
//

import Foundation
import SwiftUI

struct SkeletonCard: View {
    @State private var isAnimating = false

    var body: some View {
        RoundedRectangle(cornerRadius: 10)
            .fill(
                LinearGradient(gradient: Gradient(colors: [Color.gray.opacity(0.1), Color.gray.opacity(0.2), Color.gray.opacity(0.1)]),
                               startPoint: .leading,
                               endPoint: isAnimating ? .trailing : .leading)
            )
            .frame(height: 200)
            .padding([.top, .horizontal])
            .onAppear {
                withAnimation(Animation.easeInOut(duration: 1.0).repeatForever(autoreverses: true)) {
                    isAnimating = true
                }
            }
    }
}
