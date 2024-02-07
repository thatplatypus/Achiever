//
//  ProgressBar.swift
//  Achiever
//
//  Created by Tom Brewer on 2/4/24.
//

import Foundation
import SwiftUI

struct ProgressBar: View {
    @Binding var value: CGFloat

    var body: some View {
        GeometryReader { geo in
            ZStack(alignment: .leading) {
                Rectangle().frame(width: geo.size.width , height: geo.size.height)
                    .opacity(0.3)
                    .foregroundColor(Color(UIColor.systemTeal))

                Rectangle().frame(width: max(CGFloat(self.value)*geo.size.width, 0) , height: geo.size.height)
                    .foregroundColor(Color(UIColor.systemBlue))
                    .animation(.linear)
            }.cornerRadius(45.0)
        }
    }
}
